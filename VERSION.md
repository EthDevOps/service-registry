# Version Information System

This application includes automatic version information display in the footer, showing the git tag (semantic version) and commit hash of the build.

## How It Works

### 1. Version Service
- **`IVersionService`** and **`VersionService`** read version information from environment variables
- Displays version in format: `v1.2.3 (abc1234)` with short commit hash
- Falls back to `dev` and `unknown` for local development

### 2. Environment Variables
The following environment variables are set during Docker build:
- `APP_VERSION` - Git tag (semantic version) or branch name
- `APP_COMMIT_HASH` - Full git commit hash
- `APP_BUILD_DATE` - UTC timestamp of build

### 3. Footer Display
The footer shows:
- **Left side:** Copyright and privacy link
- **Right side:** Version info like `v1.2.3 (abc1234) | Built: 2025-01-15 10:30:45 UTC`

## Usage

### GitHub Actions (Automatic)
When you push tags with semantic versioning:
```bash
git tag v1.2.3
git push origin v1.2.3
```

The GitHub workflow will automatically:
- Extract version from tag (removes `v` prefix)
- Get short commit hash
- Set build date
- Build Docker image with version info
- Push to GitHub Container Registry

### Local Development
Use the provided build script:
```bash
# Build with auto-detected version
./build-docker.sh

# Build with specific version
./build-docker.sh 1.2.3-dev
```

### Manual Docker Build
```bash
docker build \
  --build-arg APP_VERSION="1.2.3" \
  --build-arg APP_COMMIT_HASH="$(git rev-parse --short HEAD)" \
  --build-arg APP_BUILD_DATE="$(date -u +"%Y-%m-%d %H:%M:%S UTC")" \
  -t quokka-service-registry:1.2.3 .
```

## Version Resolution Logic

| Context | Version Format | Example |
|---------|---------------|---------|
| Git tag | Tag without `v` prefix | `1.2.3` |
| Main branch | `latest` | `latest` |
| Other branches | `branch-commit` | `feature-abc1234` |
| Local dev | `dev` | `dev` |

## Files Modified

- **`Services/IVersionService.cs`** - Interface definition
- **`Services/VersionService.cs`** - Implementation reading env vars
- **`Program.cs`** - Service registration
- **`Views/Shared/_Layout.cshtml`** - Footer display
- **`Dockerfile`** - Build args and env vars
- **`.github/workflows/docker-build.yml`** - CI/CD pipeline
- **`build-docker.sh`** - Local build helper

## Semantic Versioning

Use semantic versioning for tags:
- **Major.Minor.Patch** (e.g., `v1.2.3`)
- **Pre-release:** `v1.2.3-alpha.1`
- **Build metadata:** `v1.2.3+20250115`

The system will automatically extract and display the version without the `v` prefix.