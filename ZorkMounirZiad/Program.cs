using System;
using System.Collections.Generic;

namespace Zork
{
    enum Commands
    {
        QUIT,
        LOOK,
        NORTH,
        SOUTH,
        EAST,
        WEST,
        UNKNOWN
    }

    class Program
    {
        // Dictionary to map room names to Room objects
        private static Dictionary<string, Room> RoomMap;

        // 2D Array of rooms (3x3 grid)
        private static readonly Room[,] Rooms =
        {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

        // Player starts at West of House (row 1, column 1)
        private static (int Row, int Column) Location = (1, 1);

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");
            InitializeRoomDescriptions();

            Commands command = Commands.UNKNOWN;
            Room previousRoom = null;

            while (command != Commands.QUIT)
            {
                Room currentRoom = Rooms[Location.Row, Location.Column];

                // Auto-look if we've moved to a new room
                if (currentRoom != previousRoom)
                {
                    Console.WriteLine(currentRoom);
                    Console.WriteLine(currentRoom.Description);
                }

                previousRoom = currentRoom;

                Console.Write("> ");
                command = ToCommand(Console.ReadLine()?.Trim() ?? string.Empty);

                string outputString;

                switch (command)
                {
                    case Commands.QUIT:
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.LOOK:
                        outputString = currentRoom.Description;
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        bool moved = Move(command);
                        if (moved)
                        {
                            // Movement successful → trigger auto-look next loop
                            outputString = $"You moved {command}.";
                        }
                        else
                        {
                            // No movement → no auto-look
                            outputString = "The way is shut!";
                        }
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Console.WriteLine(outputString);
            }
        }

        private static bool Move(Commands command)
        {
            int newRow = Location.Row;
            int newColumn = Location.Column;

            switch (command)
            {
                case Commands.NORTH:
                    newRow++;
                    break;
                case Commands.SOUTH:
                    newRow--;
                    break;
                case Commands.EAST:
                    newColumn++;
                    break;
                case Commands.WEST:
                    newColumn--;
                    break;
                default:
                    throw new ArgumentException("Invalid movement command.");
            }

            // Check bounds (stay within 0–2 for both row and column)
            if (newRow >= 0 && newRow < Rooms.GetLength(0) &&
                newColumn >= 0 && newColumn < Rooms.GetLength(1))
            {
                Location = (newRow, newColumn);
                return true;
            }

            return false;
        }

        private static Commands ToCommand(string commandString)
        {
            return Enum.TryParse(commandString, true, out Commands result)
                ? result
                : Commands.UNKNOWN;
        }

        private static void InitializeRoomDescriptions()
        {
            var roomMap = new Dictionary<string, Room>();

            // Populate dictionary with all room references
            foreach (Room room in Rooms)
            {
                roomMap[room.Name] = room;
            }

            // Assign descriptions using dictionary keys
            roomMap["Rocky Trail"].Description = "You are on a rocky trail. The trail heads south into dense forest.";
            roomMap["South of House"].Description = "You are facing the south side of a white house. There is no door here, and all the windows are barred.";
            roomMap["Canyon View"].Description = "You are at the top of the Great Canyon. A path leads south along the edge.";
            roomMap["Forest"].Description = "This is a forest, with trees in all directions.";
            roomMap["West of House"].Description = "This is an open field west of a white house, with a boarded front door.\nA rubber mat saying 'Welcome to Zork!' lies by the door.";
            roomMap["Behind House"].Description = "You are behind the white house. In one corner of the house there is a small window which is slightly ajar.";
            roomMap["Dense Woods"].Description = "This is a dimly lit forest, with large trees all around.";
            roomMap["North of House"].Description = "You are facing the north side of a white house. There is no door here, and all the windows are barred.";
            roomMap["Clearing"].Description = "You are in a clearing, with a forest surrounding you on all sides.";

            RoomMap = roomMap;
        }
    }

    public class Room
    {
        public string Name { get; }
        public string Description { get; set; }

        public Room(string name, string description = "")
        {
            Name = name;
            Description = description;
        }

        public override string ToString() => Name;
    }
}
