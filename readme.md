# Noldus api

# Run
You can setup the database with `docker-compose up`. <br/>
This will spin up 2 containers: <br/>
- A postgres database.
- A Adminer (dataBase viewer) on port `8080`. 

Next you can run the migrations with ` dotnet ef database update`.

If you run the app you can see its end points in the `swagger` gui.
