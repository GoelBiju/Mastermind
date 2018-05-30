using System;


namespace MasterMind
{
    class Node
    {
        /// <summary>
        /// Node data is the user's guess as a string.
        /// </summary>
        public string data;

        /// <summary>
        /// Node next points to the next Node object, if there is none 
        /// this is null by default.
        /// </summary>
        public Node next;
    }

    class List
    {
        /// <summary>
        /// The first node in the linked list data structure. 
        /// </summary>
        public Node firstNode;
    }

    class Mastermind
    {

        /// <summary>
        /// Set all elements of the white peg positions array to zero before 
        /// the game begins each time.
        /// </summary>
        /// <param name="whitePegPositions">Integer array denotes the locations of white pegs in the secret code.</param>
        /// <param name="numPositions">Integer the number of positions (length of secret code).</param>
        static void InitialiseWhitePegs(int[] whitePegPositions, int numPositions)
        {
            // Set all the elements in the white peg positions array
            // to be zero; initially zero indicates a non-white peg position.
            for (int i = 0; i < numPositions; i++)
            {
                whitePegPositions[i] = 0;
            }
        }


        /// <summary>
        /// Generate a secret code for the next mastermind game; given the 
        /// number of positions to use and the range of the number.
        /// 
        /// We make use of the in-built random library in order generate 
        /// numbers for the secret code within the given range.
        /// </summary>
        /// <param name="secretCode">Integer array the array in which to place generated numbers.</param>
        /// <param name="numPositions">Integer the number of positions (length of secret code).</param>
        /// <param name="numRange">Integer maximum number (1 up to 9) the range when generating random secret code.</param>
        /// <param name="rand">Random the in-built library used to generate random numbers.</param>
        static void GenerateSecretCode(int[] secretCode, int numPositions, int numRange, Random rand)
        {
            // Create a secret code using the random library; 
            // simulates player A in the mastermind game.
            for (int i = 0; i < numPositions; i++)
            {
                // Generates a random number from 1 to numRange.
                secretCode[i] = rand.Next(1, numRange);
            }

            Console.WriteLine("\nGenerated secret code for game.");
            Console.WriteLine("Secret code has " + numPositions + " position(s) each ranging from 1 to " + numRange + ".");
        }


        /// <summary>
        /// Takes in the user's guess code in order to evaluate against the secret code.
        /// </summary>
        /// <param name="guessCode">Integer array the array to place the input guess code.</param>
        /// <param name="numPositions">Integer the number of positions (length of secret code).</param>
        /// <param name="guessList">List the dynamic queue (implemented with linked list) to store guesses made by the user.</param>
        static void InputGuessCode(int[] guessCode, int numPositions, List guessList)
        {
            string guessEntry;
            Node guessNode = new Node();

            // Allow user (player B) to enter their guess as a string and store each
            // value into the guess code array.
            Console.WriteLine("\nEnter your guess code: ");
            guessEntry = Console.ReadLine();


            // Ensure that an entry has been made:
            if (guessEntry != "")
            {
                char guessNum;
                for (int i = 0; i < numPositions; i++)
                {
                    // Get the character slice from the string.
                    guessNum = guessEntry[i];

                    // Convert the character to a integer and store in the 
                    // appropriate element index in the guess code array.
                    guessCode[i] = guessNum - '0';
                }
            }

            // Store the user's guess into the queue.
            guessNode.data = guessEntry;
            InsertEnd(guessList, guessNode);
        }


        /// <summary>
        /// Compares both the guess and secret code looking for black pegs.
        /// 
        /// Black pegs denotes numbers which are in the same in value and located in the 
        /// same position i.e. index in both secret code and guess code arrays.
        /// </summary>
        /// <param name="secretCode">Integer array the generated code array to evaluate for black pegs.</param>
        /// <param name="guessCode">Integer array the user's guess array to evaluate for black pegs.</param>
        /// <param name="numPositions">Integer the number of positions (length of secret code).</param>
        /// <returns>Integer the number of black pegs identified from both codes.</returns>
        static int EvaluateBlackPegs(int[] secretCode, int[] guessCode, int numPositions)
        {
            int count;

            // Evaluate black pegs; secret code and guess code number has correct 
            // number at a correct position.
            count = 0;
            for (int i = 0; i < numPositions; i++)
            {
                // Compare to see if the current position contains the same number...
                if (secretCode[i] == guessCode[i])
                {
                    // ...if so, increment the black pegs count.
                    count++;
                }
            }

            return count;
        }


        /// <summary>
        /// Compares both secrete code and guess code array elements looking for white pegs.
        /// 
        /// Discounts positions with black pegs and compares each index and if they are the same 
        /// in value but not located at the same index in the arrays then they are identified
        /// to be white pegs.
        /// </summary>
        /// <param name="secretCode">Integer array the generated code array to evaluate for white pegs.</param>
        /// <param name="guessCode">Integer array the generated code array to evaluate for white pegs.</param>
        /// <param name="whitePegPositions">Integer array the array containing positions of the white pegs from the secret code.</param>
        /// <param name="numPositions">Integer the number of positions (length of secret code).</param>
        /// <returns>Integer the number of white pegs identified from both codes.</returns>
        static int EvaluateWhitePegs(int[] secretCode, int[] guessCode, int[] whitePegPositions, int numPositions)
        {
            int count;

            // Evaluate for each number in the guess code:
            count = 0;
            for (int i = 0; i < numPositions; i++)
            {
                if (guessCode[i] != secretCode[i])
                {
                    // Compare with each secret code number in position:
                    for (int j = 0; j < numPositions; j++)
                    {
                        // Check if the current number at position j for the secret code
                        // is the same as the guess code at j, meaning that it is a black peg.
                        // If this is the case, then we skip and check the next position.
                        if (guessCode[j] != secretCode[j])
                        {
                            // Check if the current position for j is 
                            // already recognised as a white peg.
                            if (whitePegPositions[j] != 1)
                            {
                                // Check if the current position is a white peg..
                                if (guessCode[i] == secretCode[j])
                                {
                                    // Increment white pegs count.
                                    count++;

                                    //.. if it is, then set the current position as a white peg
                                    // in the white peg position array.
                                    whitePegPositions[j] = 1;

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return count;
        }


        /// <summary>
        /// Returns true/false depending on if the given linked list is empty.
        /// </summary>
        /// <param name="list">List the guess list which holds the user's guesses.</param>
        /// <returns>Boolean if the guess list is currently empty or not.</returns>
        static bool IsEmpty(List list)
        {
            return (list.firstNode == null);
        }


        /// <summary>
        /// Insert a new node into the front of the linked list; used when the list 
        /// is empty and to start pointing to the new node, indicating the first item
        /// in the dynamic queue.
        /// </summary>
        /// <param name="list">List the guess list which holds the user's guesses.</param>
        /// <param name="newNode">Node the new node with guess data to be added to the guess list.</param>
        static void InsertHead(List list, Node newNode)
        {
            newNode.next = list.firstNode;
            list.firstNode = newNode;
        }


        /// <summary>
        /// Removes the first item in the dynamic queue. Equivalent to the 
        /// dequeue function in a normal queue.
        /// </summary>
        /// <param name="list">List the guess list which holds the user's guesses.</param>
        /// <returns>Node the node that was at the front of the queue.</returns>
        static Node RemoveHead(List list)
        {
            Node frontNode = list.firstNode;

            if (list.firstNode != null)
            {
                list.firstNode = list.firstNode.next;
            }

            return frontNode;
        }


        /// <summary>
        /// Inserts a node to the end of the dynamic queue; equivalent 
        /// to the add function in a normal queue which would add to the 
        /// back of the queue.
        /// </summary>
        /// <param name="list">List the guess list which holds the user's guesses.</param>
        /// <param name="newNode">Node the new node with guess data to be added to the guess list.</param>
        static void InsertEnd(List list, Node newNode)
        {
            Node node = list.firstNode;
            Node currentLastNode = node;

            if (IsEmpty(list))
            {
                InsertHead(list, newNode);
            }
            else
            {
                while (node != null)
                {
                    currentLastNode = node;
                    node = node.next;
                }

                currentLastNode.next = newNode;
                newNode.next = null;
            }
        }


        /// <summary>
        /// Prints the linked list by traversing through all
        /// the nodes which point to each other one after the other until 
        /// the end of the linked list.
        /// </summary>
        /// <param name="list">List the guess list which holds the user's guesses.</param>
        static void PrintList(List list)
        {
            Node node = list.firstNode;

            while (node != null)
            {
                Console.WriteLine(node.data);
                node = node.next;
            }
        }


        /// <summary>
        /// Entry point for the Mastermind game; invokes main functions of the game 
        /// </summary>
        static void Main(string[] args)
        {
            int numberEntry;
            int numPositions;
            int numRange;

            int blackPegs;
            int whitePegs;

            int[] secretCode;
            int[] guessCode;
            int[] whitePegPositions;

            bool gameLoop;
            bool brokenCode;

            // Use our custom dynamic queue implemented by a linked list
            // to store player B's guesses.
            List guessList = new List();

            // Use the in-built random class library to generate random numbers
            Random rand = new Random();


            // Initialise game loop to start the game and set broken code
            // to false (only true when the secret has been solved).
            gameLoop = true;
            brokenCode = false;

            Console.WriteLine("Let's play MasterMind!");
            Console.WriteLine("----------------------\n");

            // Do-while loop to control the whole program loop.
            do
            {
                // Initialise/reset the number of black and white pegs.
                blackPegs = 0;
                whitePegs = 0;

                // Allow input for the number of positions in the secret/guess code to play with.
                Console.WriteLine("\n-> Enter the number of positions for new game (positive integer): ");
                numberEntry = Convert.ToInt32(Console.ReadLine());
                if (numberEntry > 0)
                {
                    numPositions = numberEntry;
                }
                else
                {
                    Console.WriteLine("\n* Only positive integers allowed, set to 4 positions.");
                    numPositions = 4;
                }

                // Initialise the secret/guess/white peg positions arrays with the number of positions to use.
                secretCode = new int[numPositions];
                guessCode = new int[numPositions];
                whitePegPositions = new int[numPositions];

                // Allow input for the maximum range to play with (from 1 to 9).
                Console.WriteLine("\n-> Enter the range for new game (from 1 up to 9): ");
                numberEntry = Convert.ToInt32(Console.ReadLine());
                if (numberEntry > 0 && numberEntry < 10)
                {
                    numRange = numberEntry;
                }
                else
                {
                    Console.WriteLine("\n* Invalid input, range from 1 to 9 allowed only. Number range set to 9.");
                    numRange = 9;
                }

                // Reset the white peg positions array to zero.
                InitialiseWhitePegs(whitePegPositions, numPositions);

                // Generate a new secret code to play with.
                GenerateSecretCode(secretCode, numPositions, numRange, rand);

                // Intialise a loop to allow the user to keep on guess until they break the secret code:
                while (!brokenCode)
                {
                    // Display the user's previous guesses (if there any).
                    if (!IsEmpty(guessList))
                    {
                        Console.WriteLine("\nYour previous guesses:");
                        PrintList(guessList);
                    }

                    // Take the user's guess.
                    InputGuessCode(guessCode, numPositions, guessList);

                    // Evaluate the number of black pegs in the user's guess and generated secret code.
                    blackPegs = EvaluateBlackPegs(secretCode, guessCode, numPositions);
                    Console.WriteLine("\nBlack Pegs in guess code = " + blackPegs);

                    // Evaluate the number of white pegs in the user's guess and the generated secret code.
                    whitePegs = EvaluateWhitePegs(secretCode, guessCode, whitePegPositions, numPositions);
                    Console.WriteLine("White Pegs in guess code = " + whitePegs);

                    // Check if the user has won the game; when the number of black pegs
                    // is equal to the number of positions used denotes a win.
                    if (numPositions == blackPegs)
                    {
                        Console.WriteLine("\nYou've broken the code!");
                        Console.WriteLine("These were the guesses you made in the game:");

                        Node removedNode;
                        // Dequeue all the guesses from the dynamic queue.
                        while (!IsEmpty(guessList))
                        {
                            removedNode = RemoveHead(guessList);
                            Console.WriteLine(removedNode.data);
                        }

                        brokenCode = true;
                    }
                }

                // Prompt the user if they want to exit the game or not.
                string gameEntry = "";
                while (gameEntry != "y" && gameEntry != "n")
                {
                    Console.WriteLine("\n-> Would you like to play a new game? (y/n)");
                    gameEntry = Console.ReadLine();

                    // Ensure the game will quit.
                    if (gameEntry == "n")
                    {
                        gameLoop = false;
                    }
                    else
                    {
                        // Set the broken code back to false for a new game.
                        brokenCode = false;
                    }
                }
            }
            while (gameLoop);
        }
    }
}
