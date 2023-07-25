# Use the official MariaDB LTS image as the base image
FROM mariadb:lts

# Optionally, set environment variables for database configuration
ENV MYSQL_ROOT_PASSWORD=master
ENV MYSQL_DATABASE=ETLData

# Expose the default MySQL port (3306) for external connections
EXPOSE 3306

# Start the MariaDB server when the container is launched
CMD ["mysqld"]
