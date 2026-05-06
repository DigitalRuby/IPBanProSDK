#!/usr/bin/env bash
# Run the IPBanProSDK test suite under coverlet and produce raw + HTML coverage reports.
# Usage:
#   coverage/run.sh                                  # all tests
#   coverage/run.sh "FullyQualifiedName~ClientWebSocketTests"   # pass-through filter
#
# Outputs (all under the coverage/ folder so the repo root stays clean):
#   coverage/results/<guid>/coverage.cobertura.xml   raw report
#   coverage/report/index.html                       HTML report (if reportgenerator is installed)

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
cd "$REPO_ROOT"

FILTER="${1:-}"

# install reportgenerator if missing
if ! command -v reportgenerator >/dev/null 2>&1; then
  if command -v dotnet >/dev/null 2>&1; then
    echo ">> installing dotnet-reportgenerator-globaltool"
    dotnet tool install -g dotnet-reportgenerator-globaltool || true
    export PATH="$PATH:$HOME/.dotnet/tools"
  fi
fi

# clear old results so we don't aggregate across runs
rm -rf coverage/results coverage/report
mkdir -p coverage/results

echo ">> running tests with coverage collection"
TEST_ARGS=(
  "IPBanProSDKTests/IPBanProSDKTests.csproj"
  "--collect:XPlat Code Coverage"
  "--settings" "$SCRIPT_DIR/coverlet.runsettings"
  "--results-directory" "$REPO_ROOT/coverage/results"
  "-c" "Release"
  "--logger" "console;verbosity=minimal"
)
if [ -n "$FILTER" ]; then
  TEST_ARGS+=("--filter" "$FILTER")
fi
dotnet test "${TEST_ARGS[@]}"

COBERTURA="$(find coverage/results -name 'coverage.cobertura.xml' | head -n1)"
if [ -z "$COBERTURA" ]; then
  echo ">> ERROR: no coverage.cobertura.xml produced — check that coverlet.collector is referenced in IPBanProSDKTests.csproj"
  exit 1
fi
echo ">> raw report: $COBERTURA"

if command -v python3 >/dev/null 2>&1; then
  python3 "$SCRIPT_DIR/summary.py" "$COBERTURA"
fi

if command -v reportgenerator >/dev/null 2>&1; then
  echo ">> generating HTML report at coverage/report/index.html"
  reportgenerator \
    -reports:"$COBERTURA" \
    -targetdir:coverage/report \
    -reporttypes:"Html;Badges;TextSummary" \
    >/dev/null
  echo ">> open coverage/report/index.html in a browser"
else
  echo ">> reportgenerator not on PATH — skipped HTML render"
fi
