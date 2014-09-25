using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Manic_Shooter.Classes
{
    //To do: Describe class and how to use here.
    //        Outline the event idea (ex. KeyboardManager.Instance.GameKeyPressed(Key.MoveUp) += new KeyboardEvent(Keyboard_MoveUp);)

    /// <summary>
    ///  Manager for the keyboard interface. This class provides information about
    ///  the state of the keyboard and how it affects specific game functions (like
    ///  if the button for shooting is pressed or not).
    /// </summary>
    class KeyboardManager : IInputManager
    {
        /// <info>
        /// 
        ///  2 kinds of keys exist. I refer to them as GameKeys and keyboard keys. GameKeys are
        ///  game-related key functions (so MoveLeft and ShootGun would be examples of GameKeys).
        ///  Keyboard keys are the actual keyboard keys (so A and Space would be keyboard keys).
        ///  Each GameKey is mapped to a keyboard key, so we say that when the keyboard key
        ///  Left is pressed, the GameKey MoveLeft is pressed.
        /// 
        ///  There are 2 ways to access this kind of information. The first is by simply
        ///  querying the class about the state of a keyboard key or GameKey.
        ///  
        ///    KeyboardManager.Instance.IsKeyDown(Keys.Left)
        ///    
        ///    ...
        ///    
        ///    KeyboardManager.Instance.IsKeyUp(GameKeys.MoveRight)
        ///    
        ///    ...
        ///    
        ///    
        ///  The second way is to subscribe to an event that gets fired when the given
        ///  GameKey is pressed or released.
        ///  
        ///       ...   
        ///       
        ///       KeyboardManager.KeyboardEvent downPressed = KeyboardManager.Instance.GameKeyPressed(KeyboardManager.GameKeys.MoveDown);
        ///       downPressed += new KeyboardManager.KeyboardEvent(gameKey_moveDownPressed);
        ///       
        ///       ...
        ///       
        ///    }
        ///    
        ///    ...
        ///    
        ///    public void gameKey_moveDownPressed(Keys key)
        ///    {
        ///        //Move the menu selection icon one item down
        ///    }
        /// 
        ///  Evidently you have to set the GameKeyPressed to a variable before you can subscribe
        ///  to it, but other than that, it would give you a good way to handle key press/release
        ///  events quickly and mindlessly.
        ///  
        /// </info>

        /// <summary>
        /// Singleton class holder. I have a thing for
        ///  singletons, unfortunately.
        /// </summary>
        private static KeyboardManager _instance = null;

        /// <summary>
        /// All the different GameKey functions that can have
        ///  keyboard keys mapped to them. Note that multiple
        ///   GameKeys can be mapped to different keys!
        ///    (Such that we could have a PlayerMoveUp and a MenuMoveUp)
        /// </summary>
        public enum GameKeys
        {
            MoveLeft = 0,
            MoveRight = 1,
            Moveup = 2,
            MoveDown = 3,
            Shoot = 4
        }

        /// <summary>
        /// The default keys that the game
        ///  will initially load for the player
        /// </summary>
        private Keys[] _defaultGameKeyMappings = new Keys[]
        {
            Keys.Left,
            Keys.Right,
            Keys.Up,
            Keys.Down,
            Keys.Space
        };

        /// <summary>
        /// A count of all GameKeys. Need to update this
        ///  when adding in a new GameKey!
        /// </summary>
        public const int GameKeysCount = 5;

        /// <summary>
        /// A Keyboard event. When a GameKey is pressed or released,
        ///  it calls this event and passes the key that was pressed.
        /// </summary>
        public delegate void KeyboardEvent(Keys key);

        /// <summary>
        /// A list of Keyboard Events that get called when a
        ///  Gamekey gets pressed. Accessed through the
        ///  GameKeyPressed method.
        /// </summary>
        private event KeyboardEvent[] _gameKeyPressed;

        /// <summary>
        /// A list of Keyboard Events that get called when a
        ///  Gamekey gets released. Accessed through the
        ///  GameKeyReleased method.
        /// </summary>
        private event KeyboardEvent[] _gameKeyReleased;

        /// <summary>
        /// A list of mappings of keyboard keys to GameKeys.
        ///  Accessed through GetGameKeyMapping and
        ///   SetGameKeyMapping methods.
        /// </summary>
        private Keys[] _gameKeyMappings;

        /// <summary>
        /// Holds the previous state of the keyboard 
        ///  (which keys were down and which were up)
        /// </summary>
        private KeyboardState _lastKeyboardState;

        /// <summary>
        /// Singleton access to the KeyboardManager class. Allows any
        ///  object to acces the same KeyboardManager through KeyboardManager.Instance
        /// </summary>
        public static KeyboardManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KeyboardManager();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Constructor for the Keyboard Manager. Initializes
        ///  the events and sets the current key mappings to the defaults.
        /// </summary>
        public KeyboardManager()
        {
            _gameKeyPressed = new KeyboardEvent[GameKeysCount];
            _gameKeyReleased = new KeyboardEvent[GameKeysCount];
            _gameKeyMappings = new Keys[GameKeysCount];

            for (int i = 0; i < GameKeysCount; i++)
            {
                _gameKeyPressed[i] = delegate { };
                _gameKeyReleased[i] = delegate { };
                _gameKeyMappings[i] = _defaultGameKeyMappings[i];
            }

            _lastKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Used to update the Manager and see the latest changes
        /// in the user's input.
        /// </summary>
        /// <param name="gameTime">The amount of time elapsed since the last call</param>
        public void Update(GameTime gameTime)
        {
            KeyboardState newKeyboardState = Keyboard.GetState();
            

            for (int i = 0; i < GameKeysCount; i++)
            {
                bool isKeyDown = newKeyboardState.IsKeyDown(_gameKeyMappings[i]);
                if (isKeyDown != _lastKeyboardState.IsKeyDown(_gameKeyMappings[i]))
                {
                    if (isKeyDown)
                    {
                        _gameKeyPressed[i](_gameKeyMappings[i]);
                    }
                    else
                    {
                        _gameKeyReleased[i](_gameKeyMappings[i]);
                    }
                }
            }

            _lastKeyboardState = newKeyboardState;
        }

        /// <summary>
        /// Tells the user if the desired keyboard key is currently pressed
        /// </summary>
        /// <param name="key">The selected keyboard key</param>
        public bool IsKeyDown(Keys key)
        {
            return _lastKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Tells the user if the desired keyboard key is currently released
        /// </summary>
        /// <param name="key">The selected keyboard key</param>
        public bool IsKeyUp(Keys key)
        {
            return _lastKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Tells the user if the desired GameKey is currently pressed
        /// </summary>
        /// <param name="key">The selected GameKey</param>
        public bool IsKeyDown(GameKeys key)
        {
            return _lastKeyboardState.IsKeyDown(_gameKeyMappings[(int)key]);
        }

        /// <summary>
        /// Tells the user if the desired GameKey is currently released
        /// </summary>
        /// <param name="key">The selected GameKey</param>
        public bool IsKeyUp(GameKeys key)
        {
            return _lastKeyboardState.IsKeyDown(_gameKeyMappings[(int)key]);
        }

        /// <summary>
        /// Used to retrieve the GameKeyReleased event for the given GameKey
        /// </summary>
        /// <param name="key">The selected GameKey</param>
        public KeyboardEvent GameKeyPressed(GameKeys key)
        {
            return _gameKeyPressed[(int)key];
        }

        /// <summary>
        /// Used to retrieve the GameKeyReleased event for the given GameKey
        /// </summary>
        /// <param name="key">The selected GameKey</param>
        public KeyboardEvent GameKeyReleased(GameKeys key)
        {
            return _gameKeyReleased[(int)key];
        }

        /// <summary>
        /// Used to retrieve the key mapping for the given Gamekey.
        /// </summary>
        /// <param name="key">The GameKey that the user wants to see the key mapping for</param>
        public Keys GetGameKeyMapping(GameKeys key)
        {
            return _gameKeyMappings[(int)key];
        }

        /// <summary>
        /// Used to set the key mapping for the given Gamekey.
        /// </summary>
        /// <param name="key">The GameKey that the user wants to change the key mapping for</param>
        public void SetGameKeyMapping(GameKeys gameKey, Keys newKey)
        {
            _gameKeyMappings[(int)gameKey] = newKey;
        }

    }
}
