#!/usr/bin/env bash

set -euxo pipefail

pushd ./deployment
yarn install
yarn build
yarn deploy
popd
