# KitchenRoutingSystem

This is a project developed in C# project using .Net Core 3.1 framework

## Request Data
It´s a Web API that use json structure to make an order to "POS", follow the json structure example:
{
   "products":[
      {
         "productId":"1",
         "value":1,
         "quantity":2,
         "productType":1,
         "status":1
      }
   ]
}

## Rabbit Container
To manager the queues I opted to use a RabbitMQ, to mount your container in Docker I used this command line:

docker run -d --hostname rabbit-local --name testes-rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=testes -e RABBITMQ_DEFAULT_PASS=Testes2018! rabbitmq:3-management


