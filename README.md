# UnityGameManager
A few simple game management components for the unity engine.


## Game Controller:
Responsible for pausing/resuming the game.

*(only one instance allowed per scene)*


  ### Pausing/Resuming

  * pausing the game:

        GameController.PauseGame();

  * resuming the game:

        GameController.ResumeGame();


  ### Callbacks
  The callback methods should have no parameters
  
  * to receive a callback when game is paused:

        GameController.OnPause += SomeMethod

  * to receive a callback when game resumes:

        GameController.OnResume += SomeMethod

    *use -= to stop receiving callbacks*
---

## Game Clock
Keeps a log of 'game world time'.
Intended for RTS games that require their own time system.

*(only one instance allowed per scene)*

  ### Config:
  * __Stop On Pause (bool):__ should the time stop accumulating when game is paused?
  
  * __Base Multiplier (float):__ the number of times faster game time is that real time.
  
  * __Speed Multipliers (float[]):__ an array of speeds in ascending order, starting at 0
  
  * __Start Date (Date):__ the date and time when the game is started
  
  
  ### Getting the time:
  * to get a DateTime struct with the current game time (get only):
  
        GameClock.DateTime
      
  * to get a TimeSpan struct representing the time passed since the start date
  
        GameClock.Epoch
  
---
  
## PersistenceManager
Saves and loads data for the entire game.
Supports multiple game saves.
Will also serialize pretty much anything (as long as its serializable)
  
*(only one instance allowed per scene)*

  ### Serialization
  * to serialize sometime to a byte array:
  
        PersistenceManager.Serialize(T object)
        
  * to deserialize from byte array to object:
  
        PersistenceManager.DeSerialize<T>(byte[] bytes)
  
  ### Saving/Loading

  * creating a new game save:
  
        PersistenceManager.NewGameSave(string gameSaveName)

  * saving the game:

        PersistenceManager.SaveGame(string gameSaveName);

  * resuming the game:

        PersistenceManager.LoadGame(string gameSaveName);
  
  ### Callbacks
  callback methods should take Dictionary<string, byte[]> as their only parameter.
  game data is usually stored by some dort of manager class, so the key would almost always be
  
        GetType().ToString()
        
  and the byte array would be a serialized something.
  
  
  * to receive a callback when game is saving:

        PersistenceManager.OnSave += SomeMethod

  * to receive a callback when game is loading:

        PersistenceManager.OnLoad += SomeMethod

    *use -= to stop receiving callbacks*
  
---
  
## PlayTimeCounter
Keeps a log of time user has spent playing the game.

*(only one instance allowed per scene)*

  ### Config:
  * __Stop On Pause (bool):__ should the time stop accumulating when game is paused?
  
  ### Getting Play Time;
  * to get a TimeSpan representing the amount of time the game has been played
  
        PlayTimeCounter.PlayTime

---
