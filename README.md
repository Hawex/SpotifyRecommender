# SpotifyRecommender
To run the project you need to:
1. Go to {repository}\Remotes\SpotifyAPI\Auth\IdentityClient.cs and set proper ClientId and ClientSecret
2. Go to {repository}\Services\RecommenderService\RecommenderService.API\appsettings.json and set RecommenderServiceRepositoryDbContext according to schema:
"Server={serverAddress},{port};Database=SpotifyRecommenderDb;User ID={userLogin};Password={userPassword}"
3. Right click on solution in VS -> Select Startup projects -> Check "Multiple startup projects" -> set action "Start" on "RecommenderService.Api", "SpotifyRecommender.BFF" and "SpotifyRecommender.WebApp".
4. Run with configuration "<Multiple startup projects>"

To generate new recommendations for user you need to:
1. Go to "Add user"
2. Provide unique user name
3. Go to "Home"
4. Select user, favourite genres, tracks and artists.
5. Click "Save user preferences"
6. Go to "Recommendations"
7. Select user and click "Get user recommendations."
This will generate list of 10 recommendations based on user preferences.


In this project there is a base to update user recommendations, but because lack of time it was not completed. The plan for this was to add "Listen" button to every recommended track, and after that new track should be added to "RecentlyListenedSongs" to database, and there should be a popup "How do you like this song" with rating 0-5. Rating should increase or decrease score of artist, genre and track in "UserFavourites" table in database. 

The "UserRecommendations" table was created to cache recommendations for user in future, when "GenerateRecommendations" method will be asynchronous.

