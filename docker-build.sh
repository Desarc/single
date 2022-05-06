export COMMIT_ID=$( git rev-parse --short HEAD )

echo "Commit ID: $COMMIT_ID"

docker-compose build

docker-compose push