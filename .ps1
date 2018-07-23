# © Alexander Kozlenko. Licensed under the MIT License.

function .($_) { & $_; if ($lastexitcode) { exit $lastexitcode } }

.{ dotnet clean --verbosity minimal }
.{ dotnet build --verbosity minimal }
.{ dotnet test --no-build --verbosity minimal }
.{ dotnet pack --no-build --verbosity minimal }