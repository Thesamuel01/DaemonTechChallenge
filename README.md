# Brazilian Hedge Funds Data Analysis

This project is part of the Daemon Investments tech challenge.

## The Challenge

Our research team needs to conduct analysis on Brazilian hedge funds, and to do so, they require historical data of these funds. The goal of this challenge is to create a simple ETL (Extract, Transform, Load) process that fetches, organizes, and persists hedge fund daily data from CVM (Comissão de Valores Mobiliários), the Brazilian Securities and Exchange Commission.

Once the data is successfully stored, your task is to develop an API that enables other users, particularly the research team, to query and access the fetched data. The data should be returned sorted by date and should contain all the relevant fields.

### Data Source

The data source used for this challenge is available at:

[http://dados.cvm.gov.br/dados/FI/DOC/INF_DIARIO/DADOS/](http://dados.cvm.gov.br/dados/FI/DOC/INF_DIARIO/DADOS/)

### API Endpoint

```http
GET /Report
```
The API endpoint to access the fetched data. The following parameters are accepted for querying:

- **CNPJ**: Mandatory parameter. The data retrieved will belong to the specified CNPJ, using the `CNPJ_FUNDO` field from the source CSV file.

- **Start Date**: Optional parameter. Filter the data for dates greater than or equal to the specified Start Date.

- **End Date**: Optional parameter. Filter the data for dates lesser than the specified End Date.

#### Response

**200 OK - Success**

```json
[
	{
		"id": 1,
		"cnpjFundo": "00.017.024/0001-53",
		"dtComptc": "2017-01-02T00:00:00",
		"vlTotal": 1082310.35,
		"vlQuota": 24.264750500000,
		"vlPatrimLiq": 1080998.58,
		"captcDia": 0.00,
		"resgDia": 0.00,
		"nrCotst": 1
	},
	{
		"id": 2,
		"cnpjFundo": "00.017.024/0001-53",
		"dtComptc": "2017-01-03T00:00:00",
		"vlTotal": 1082843.72,
		"vlQuota": 24.274862900000,
		"vlPatrimLiq": 1081449.09,
		"captcDia": 0.00,
		"resgDia": 0.00,
		"nrCotst": 1
	},
	/.../
]
```

**400 Bad Request - Validation Error**

```json
{
	"type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
	"title": "One or more validation errors occurred.",
	"status": 400,
	"traceId": "00-62188212bc32ddfa7ae18271fc8b896e-b0556bfff225bf08-00",
	"errors": {
		"CNPJ": [
			"The CNPJ field is required."
		]
	}
}

```

### Preparing the Environment

Before running the ETL process and the API, make sure you have a running instance of MariaDB LTS. If you prefer, you can use Docker to set up the database.


#### Run Docker Compose:
```bash
docker compose -f docker-compose.dev.yml up -d
```

### Running the Project

This project is built using ASP.NET 6.0.412. To start the ETL process and the API, follow these steps:

1. Ensure that the database is up and running.
2. Navigate to the project directory of the challenge solution.
3. Execute `dotnet build` to build the project.
4. Execute `dotnet run` to start the server.

The server will begin by running the ETL process first. After the data is loaded, the API will be accessible at [http://localhost:5104](http://localhost:5104).

### Note on ETL Process

As the ETL process can be resource-intensive, it is recommended to have a minimum of 4GB of available RAM to ensure smooth execution.

Feel free to reach out if you have any questions or face any issues. Happy data analysis! :chart_with_upwards_trend:
