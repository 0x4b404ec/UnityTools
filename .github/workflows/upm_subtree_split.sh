UPM_BRANCH_NAME="upm"
SEMVER_REG_EXP="[0-9]+.[0-9]+.[0-9]+"

VERSION_JSON=$(cat ./Assets/Core/package.json | grep version)
echo $VERSION_JSON
VERSION=$(echo $VERSION_JSON | grep -Eo $SEMVER_REG_EXP)
echo $VERSION
echo "==============================================================="

git push origin --delete $VERSION

git checkout -b $UPM_BRANCH_NAME
git checkout master
git subtree split --prefix=Assets/Core --branch $UPM_BRANCH_NAME
git tag $VERSION $UPM_BRANCH_NAME
git push origin -f $UPM_BRANCH_NAME --tags