# Prime Number Validator
This repository contains a gRPC service for validating if a number is prime or not, as well as a client for load testing this service. It is implemented using .NET.

## Build and Publish

### Build
To build the project, use the following command in the project root directory:
```bash
make build
```
This will build the project using `dotnet build`.

### Publish
To publish the project, run the following command. It will publish both the server and client binaries into the `binaries` folder:
```bash
make publish
```
This will produce client and server binaries with the configuration set to Release, using the current runtime and making them self-contained. Debug information and symbols will be excluded from the output.
You can find the published binaries in the `binaries` folder.

### Clean
To clean the project and remove build artifacts, use the following command:
```bash
make clean
```

## Server Usage

The `server` binary can be used to start the Prime Number verification web service. 
To run the server, use the following command:
```bash
cd ./binaries/  # Please make sure you run server in it's folder context
./server 
```
Here are the available options:
```
server [options]

Options:
--urls <urls> A semi-colon delimited list of URL(s) to configure for the web server [default: http://localhost:4242]
--limit <limit> Max supported input number. May influence startup time with large values [default: 1000]
--output-delay <output-delay> Console output refresh rate in seconds [default: 1]
--version Show version information
-?, -h, --help Show help and usage information
```
The server will output statistics in the console every second (with `--output-delay 1`), as shown below:
```
UTC Time: <Current time>
Requests per second: <RPS rate for last second>
Requests Count: <Total requests count>
Most requested prime numbers: <Top 10 requested prime numbers>
<Number> : <Count>
...
```
Please note that the server binary should be started in the context of the binaries folder because it requires the appsettings.json file. This is a known issue that has not been fixed at the moment.

## Client Usage
The `client` binary is used for load testing the Prime Number web service. 

To run the client binary, use the following command:
```bash
./binaries/client
```
Here are the available options:
```
client [options]

Options:
--url <url> Service URL [default: http://localhost:4242]
--time <time> Test execution time [default: 1]
--rate <rate> Target requests per second rate [default: 10000]
--limit <limit> Max supported input number. May influence startup time with large values [default: 1000]
--version Show version information
-?, -h, --help Show help and usage information
```
Feel free to customize the options to fit your testing needs.

The client will print a line for each executed request, and after the execution of all requests, it will print a summary, as shown below:
#### Request Output (for each request):
`# <ID> :: N <Number> :: RTT <Round trip time> ms :: Prime: <True|False - server result> :: <Valid|Invalid - client validation result>` 

#### Summary Output (after all requests):
```
Execution Complete
Requests Sent: <Total count of sent requests>
Failed Requests: <Count or requests with Status code != 0>
Average Round Trip Time: <Average RTT>
Time Started: <Execution start time> :: Time Finished: <Execution finish time> :: Execution Time: <Execution period>
Target Rate: <Requests per second passed in args>
Actual Average Rate: <Actual average performed rate>
Requests summary: <Summarized by Status code requests counts>
:: Status: <Status code> :: <Status name> :: Count: <Requests count> ::
...
```