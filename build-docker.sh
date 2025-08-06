#!/bin/bash

# Local Docker build script with version information
# Usage: ./build-docker.sh [tag-name]

set -e

# Get version information
COMMIT_HASH=$(git rev-parse --short HEAD)
BUILD_DATE=$(date -u +"%Y-%m-%d %H:%M:%S UTC")

# Determine version
if [ -n "$1" ]; then
  VERSION="$1"
else
  # Try to get the latest git tag, fallback to branch name
  if git describe --tags --exact-match HEAD 2>/dev/null; then
    VERSION=$(git describe --tags --exact-match HEAD | sed 's/^v//')
  else
    BRANCH=$(git branch --show-current)
    VERSION="${BRANCH:-dev}-${COMMIT_HASH}"
  fi
fi

echo "🏗️  Building Docker image with version information:"
echo "   Version: $VERSION"
echo "   Commit: $COMMIT_HASH" 
echo "   Build Date: $BUILD_DATE"
echo ""

# Build the Docker image
docker build \
  --build-arg APP_VERSION="$VERSION" \
  --build-arg APP_COMMIT_HASH="$COMMIT_HASH" \
  --build-arg APP_BUILD_DATE="$BUILD_DATE" \
  -t "quokka-service-registry:$VERSION" \
  -t "quokka-service-registry:latest" \
  .

echo ""
echo "✅ Docker image built successfully!"
echo "   Tags: quokka-service-registry:$VERSION, quokka-service-registry:latest"
echo ""
echo "🚀 To run the container:"
echo "   docker run -p 8080:8080 quokka-service-registry:latest"