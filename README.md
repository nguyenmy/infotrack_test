# SettlementBooking
This project is a .NET Core API for managing bookings within specified business hours. It allows users to create bookings, ensuring no more than 4 bookings occur simultaneously during any given hour. The API includes validation, error handling, and provides documentation via Swagger.

## API

`BookingController` provides an implementation of booking business

## How to execute

1. Create a successful booking:

```
curl --request POST \
  --url https://localhost:44355/Booking \
  --header 'Content-Type: application/json' \
  --data '{
	"bookingTime": "09:00",
	"name": "Keith"
}'
```
Should return a status code 200 with a Booking Id
```
{
 "bookingId": "d90f8c55-90a5-4537-a99d-c68242a6012b"
}
```
2. Create a booking that is out of business hours

```
curl --request POST \
  --url https://localhost:44355/Booking \
  --header 'Content-Type: application/json' \
  --data '{
	"bookingTime": "08:00",
	"name": "Keith"
}'
```
Should return a bad request error with an error message details
```
{
  "type": "https://httpstatuses.com/400",
  "title": "Bad Request",
  "status": 400,
  "detail": "Booking time is out of business hours",
  "instance": null,
  "traceId": "00-7ea3d8133882594798c5beb72a7cb865-707c70d50685b842-00"
}
```
3. Create a booking with an empty name

```
curl --request POST \
  --url https://localhost:44355/Booking \
  --header 'Content-Type: application/json' \
  --data '{
	"bookingTime": "10:00",
	"name": ""
}'
```
Should return a bad request error with an error message details
```
{
  "type": "https://httpstatuses.com/400",
  "title": "Bad Request",
  "status": 400,
  "detail": "'Name' must not be empty.",
  "instance": null,
  "traceId": "00-377caa2af2c0534eb4d7a42d364b487b-46938fdd13bbbd49-00"
}
```
4. Create more than 4 bookings at the same time. 

```
curl --request POST \
  --url https://localhost:44355/Booking \
  --header 'Content-Type: application/json' \
  --data '{
	"bookingTime": "10:00",
	"name": "Keith"
}'
```
Should return a conflict error with an error message details
```
{
  "type": "https://httpstatuses.com/409",
  "title": "Conflict",
  "status": 409,
  "detail": "All slots are booked for this time.",
  "instance": null,
  "traceId": "00-d3a423424435c54893fd1f9f6a80709c-0e0d456f4a801d48-00"
}
```
