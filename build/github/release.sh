#!/bin/bash

#GITHUB_ACCESS_TOKEN=""
GITHUB_API="https://api.github.com"
GITHUB_OWNER="egramtel"
GITHUB_REPO="$GITHUB_API/repos/$GITHUB_OWNER/egram.tel/releases?access_token=$GITHUB_ACCESS_TOKEN"
#VERSION="1.0"
#DESCRIPTION="-"

# create release
RELEASE_JSON=$(printf '{"tag_name": "v%s","target_commitish": "master","name": "egram-%s","body": "%s","draft": false,"prerelease": false}' "$VERSION" "$VERSION" "$DESCRIPTION")
echo "Creating a release with info: $RELEASE_JSON."

release_id="$(curl --data "$RELEASE_JSON" $GITHUB_REPO | python -c "import sys, json; print(json.load(sys.stdin)['id'])")"

# exit script if release wasn't created
if [ -z "$release_id" ]
then
    echo "Failed to create release."
    exit 1
else
    echo "Release created with id: $release_id."
fi

f="$SYSTEM_ARTIFACTSDIRECTORY/_egram/image/Egram.dmg"
echo $f
GITHUB_ASSET="https://uploads.github.com/repos/$GITHUB_OWNER/egram.tel/releases/$release_id/assets?name=$(basename "$f")&access_token=$GITHUB_ACCESS_TOKEN"
curl --data-binary @"$f" -H "Content-Type: application/octet-stream" "$GITHUB_ASSET"

f="$SYSTEM_ARTIFACTSDIRECTORY/_egram/tarball/egram-x64.tar.gz"
echo $f
GITHUB_ASSET="https://uploads.github.com/repos/$GITHUB_OWNER/egram.tel/releases/$release_id/assets?name=$(basename "$f")&access_token=$GITHUB_ACCESS_TOKEN"
curl --data-binary @"$f" -H "Content-Type: application/octet-stream" "$GITHUB_ASSET"

f="$SYSTEM_ARTIFACTSDIRECTORY/_egram/installer/egram-setup.exe"
echo $f
GITHUB_ASSET="https://uploads.github.com/repos/$GITHUB_OWNER/egram.tel/releases/$release_id/assets?name=$(basename "$f")&access_token=$GITHUB_ACCESS_TOKEN"
curl --data-binary @"$f" -H "Content-Type: application/octet-stream" "$GITHUB_ASSET"

f="$SYSTEM_ARTIFACTSDIRECTORY/_egram/installer/egram.zip"
echo $f
GITHUB_ASSET="https://uploads.github.com/repos/$GITHUB_OWNER/egram.tel/releases/$release_id/assets?name=$(basename "$f")&access_token=$GITHUB_ACCESS_TOKEN"
curl --data-binary @"$f" -H "Content-Type: application/octet-stream" "$GITHUB_ASSET"

echo "Success!"
exit 0
