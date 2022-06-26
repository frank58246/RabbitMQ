$ver = "0627"
docker login
docker build -t frank58246/rabbitmq-poc:$ver -f RabbitMQ/Dockerfile . 
docker build -t frank58246/rabbitmq-consumer:$ver -f RabbitMQ.Consumer/Dockerfile .
docker push frank58246/rabbitmq-poc:$ver
docker push frank58246/rabbitmq-consumer:$ver
