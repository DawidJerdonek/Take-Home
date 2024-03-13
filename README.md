# Take-Home Assignment

## BrainCloud Integration With Game Project

### Project Overview:
A project which interacts with brainCloud backend using the brainCloud package.
The project is a Cookie Clicker game where the player is shown how many times they have clicked a cookie,
and how long they have been clicking for. Both these values are stored by utilising brainCloud.
Utilising Leaderboards, Statistics, Achievements, Authentication

### Project Features:
Projects integrated with brainCloud Unity SDK
The project is made up of 3 Scenes. Main Menu, Login, Gameplay
The project utilises Universal Authentication for players.
Players can log in using a username and password while also being able to log out.
The project contains leaderboards which store player highscores.
Players can submit their scores to the leaderboards by entering a name and clicking Submit Highscore from the Gameplay Scene.
Players are able to click a cookie, the idea similar to many games of the "Cookie Clicker" genre.
Players can earn achievements which in turn unlock permanent upgrades for the game.(eg. after 100 cookies, players get two cookies for each click)
Players can save their progress to brainCloud by clicking save or save and exit.
Players can view the leaderboard, statistics, and achievements from the menu.
Players can chat with each other(Utilising Leaderboards)

#### To showcase custom functionality and creative flair:
Implemented a chat utilising Leaderboards rather than any built-in messaging/chat tools.
Implemented a permanent upgrade system utilising Achievements.
Integrated brainCloud into a Cookie Clicker game.  

### How to Demo:
!Note!
---All testing of application should be done starting in the MainMenu Scene---
---The game will not work as intended if it is played from a different scene---

#### Client
1. Clone GitHub repo to computer and open using Unity.
2. Open the MainMenu Scene
3. Run the Scene.
4. Press the Start Button
5. When prompted to Log in, create an account or use one which I used for testing (username: hello password: hello)(username: 12345 password: 12345)
6. Once logged in, you may check the leaderboards, player statistics and achievements by clicking each corresponding button.
7. When ready click the Play Game button.
8. You can now play the cookie clicker game, depending on what Achievements are unlocked, certain upgrades are unlocked to help you get more cookies faster!
9. Click the Cookie a few times!
10. Click Save Game to save user statistics
11. Enter a username into the top right input field(This username will be the displayed username on the leaderboard). Click the Submit Highscore button.
12. Click the View Chat button(This is the game chat, any player can type into this chat).
13. Type a nice message into the chat input field and click send. The message should show up at the top of the chat.
14. Click the X button over the chat to close the chat window.
15. Click the Save and Exit button.
16. You will be back in the Main Menu now, click Start.
17. Click the Log Out button in the bottom right of the screen.
18. Once logged out you will be back in the Main Menu, click the Start button.
19. Now when prompted to Log In, log in with a different account, a new one or one of the two which I used for testing.(username: hello password: hello)(username: 12345 password: 12345)
20. Once logged in once again check the Leaderboards, Statistics, and Achievements. The latest highscore that was uploaded by you should be visible in the leaderboards. The statistics and achievements displayed belong to the player currently logged in.
21. Click the Play Game button.
22. Click the Cookie a few times.
23. Click the View Chat button, notice that the messages sent from the previous account are there.
24. Write a message in the input field and click the Send button, click the X button.
25. Type in a username in the top right input field and click Submit Highscore.
25. Click Save and Exit.
26. Click Start.
27. Click Statistics, all statistics and achievements should be up to date from when you have clicked the Save and Exit button.
28. Click Achievements to view Achievements.
29. Click Leaderboard, the score that was submitted by the player should show up in the leaderboard

#### Backend
1. All of the data stored on the backend was already showcased in the game, however all this data can be viewed on brainCloud itself.
2. Go to https://getbraincloud.com/ and click Log In. 
3. Log In using the credentials supplied in the email.
4. Choosed the team: DawidJ
5. Click on App in the top left, navigate to Design -> Cloud Data -> User Statistics. (This is where the User Statistics are set up)
6. Now navigate to Design -> Gamification -> Achievements. (This is where all the achievements used in the game are)
7. Now navigate to Design -> Leaderboards -> Leaderboard Configs. (This is where the leaderboards used in the game are)
8. Click on the highscore leaderboard, click on View Scores on the right side of the pop-up. This is where the scores are.
9. Click on one of the player ids, the nickname chosen by the player will be displayed in the pop-up.
10. Now navigate to Users -> User Browser. (This is where all the profile ids of different players are stored).
11. Now navigate to Design -> Cloud Code -> API Explorer. Here we can perform different operations on the backend.
12. In Service select Authenticate from the drop-down menu. Then select Authenticate in the Operation drop-down menu. Click Execute.
13. In Service select Leaderboard from the drop-down menu. Then select GetGlobalLeaderboardPage in the Operation drop-down menu.
14. In Parameters change the leaderboardId : "default" to "Highscore". Click Execute.
15. The members of the leaderboard should be logged and displayed. (If at any point one of the operations logs an error, repeat step 12.)
16. In Service select Leaderboard from the drop-down menu. Then select PostScoreToLeaderboard in the Operation drop-down menu.
17. In Parameters change leaderboardId : "default" to "Highscore". Change the score to a number such as 20 and change the nickname to a desired name such as "John". Click Execute.
18. The response should read status: 200. Now select GetGlobalLeaderboardPage in the Operation drop-down menu. In Parameters change the leaderboardId : "default" to "Highscore". Click Execute.
19. The leaderboard should now have a member with the nickname and score provided.
20. In Service select Gamification from the drop-down menu. Then select ReadAchievements in the Operation drop-down menu. Click Execute.
21. This should log all of the Achievements obtainable in the game.
22. In Service select PlayerStatistics from the drop-down menu. Then select ReadAllUserStats in the Operation drop-down menu. Click Execute.
23. This should log the stats of the account that is logged into brainCloud.

### What I found easy:
1. I liked how easy to navigate and quite user friendly brainCloud is.
2. I think the documentation and explanations that brainCloud provides for their service are incredibly good, intuitive and quite comprehensive.
3. Implementing user statistics and achievements was straight forward and easy to understand.

### What I found difficult:
1. Implementing Leaderboards into the project and understanding how they are handled. 
2. Understanding how the data is stored on brainCloud.

### What skills I have learned:
1. How to set up brainCloud backend for a game. 
2. BrainCloud is incredibly valuable and impressive in terms of features and tools which it provides.
3. How to request data from brainCloud.
4. How to send data from client to brainCloud leaderboards.
5. How to implement and display statistics for users.
6. How to implement and display achievements for users.
7. How I can apply permanent upgrades in a game to a user, based on achievements.
8. How to create a basic chat using Leaderboards.

### Video Link:

