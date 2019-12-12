# uConf

Absurdly cloud-native Conference Planner.

## Prerequisites

* A Kubernetes cluster
* [Skaffold](https://skaffold.dev/)

## Running locally

**Requires:** A local kubernetes cluster, configured to use your local Docker daemon. Docker Desktop's default config will work for this.

1. Set your active `kubectl` context to your cluster and a namespace unique to this app (we recommend the name `uconf`)
1. Run `skaffold run`
1. Open `http://localhost:8080` to view the frontend
    * Services are available at `/api/v1/services/[serviceName]`