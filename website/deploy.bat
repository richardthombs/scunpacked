@call build.bat
kubectl -n scdb delete deployment -l app=scdb -l tier=website
kubectl -n scdb apply -f ..\k8s
