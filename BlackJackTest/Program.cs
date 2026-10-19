using System.Globalization;

Random rng = new Random();
int[] playerDeck = new int[2];
int[] dealerDeck = new int[1];
decimal balance = 1500;
bool gameActive = true;
bool gameOver = true;
bool canChangeBet = true;

// Pre Game
System.Console.WriteLine("---------------------------------------------");
System.Console.WriteLine("\nWelcome to Blackjack!\n");
System.Console.WriteLine("---------------------------------------------\n");

Console.WriteLine($"To begin, please place a bet.");
decimal bet = BetSystem(balance, 0, true);
    

// Game
while(gameActive)
{
    while(!gameOver)
    {
    bool blackJack = false;
    System.Console.WriteLine($"Balance: {balance:C}");
    System.Console.WriteLine($"Current Bet: {bet:C}\n");
    DeckDisplay(playerDeck, "Your");
    DeckDisplay(dealerDeck, "Dealer");
    System.Console.WriteLine(" ");
    int userInput = UserInput();

    if(userInput == 5) // Help
    {
        Help();
    }

    if(userInput == 4 && canChangeBet) // Alter Bet
    {
        bet = BetSystem(balance, bet, false);
    }
    
    else if(userInput == 4 && !canChangeBet)
    {
        System.Console.WriteLine("Sorry, you've already began playing and cannot alter your bet.");
    }
    
    if(userInput == 1) // Draw
    {
        canChangeBet = false;
        playerDeck = DrawCard(playerDeck);
        if(playerDeck.Sum() == 21)
        {
            DeckDisplay(playerDeck, "Your");
            System.Console.WriteLine("\n---------------------------------------------\n");
            blackJack = true;
        }
        
        if(playerDeck.Sum() > 21)
        {
            OutputMessage(playerDeck, dealerDeck);
            balance += CardCheck(playerDeck, dealerDeck, bet);
            System.Console.WriteLine($"New Balance: {balance:C}\n");
            gameOver = true;
        }
        
    } 
    if(userInput == 0) // End Game
    {
        gameOver = true;
    }
    if(userInput == 2 || blackJack) // Stand
    {
        dealerDeck = DealerDraw(dealerDeck);
        Thread.Sleep(700);
        System.Console.WriteLine("---------------------------------------------\n");
        OutputMessage(playerDeck, dealerDeck);
        balance += CardCheck(playerDeck, dealerDeck, bet);;
        System.Console.WriteLine($"New Balance: {balance:C}\n");
        gameOver = true;



    }
    if(userInput == 3) // Double
    {
        if(bet*2 < balance && canChangeBet)
        {
        playerDeck = DrawCard(playerDeck);
        DeckDisplay(playerDeck, "Your");
        
        bet *= 2;
        System.Console.WriteLine($"Doubled Bet: {bet:C}");
        System.Console.WriteLine("\n---------------------------------------------\n");
        Thread.Sleep(1500);
        dealerDeck = DealerDraw(dealerDeck);
        Thread.Sleep(700);
        
        System.Console.WriteLine("---------------------------------------------\n");
        balance += CardCheck(playerDeck, dealerDeck, bet);
        OutputMessage(playerDeck, dealerDeck);
        System.Console.WriteLine($"New Balance: {balance:C}\n");
        
        bet /= 2;
        gameOver = true;
        }
        if(!canChangeBet || !(bet*2 < balance))
        {
            System.Console.WriteLine("Sorry, you cannot double you bet at this time.");
        }
    }
    if(bet > balance)
    {
        balance = 100;
        bet = 10;
        System.Console.WriteLine("You went bankrupt! Here's a dollar.");
        Console.WriteLine($"New Balance: {balance:C}\n");
    }
    
    }
    
    if (gameOver)
    {
        System.Console.WriteLine("To start a new game, enter 1. To exit, enter 0. For help, enter 5.\n");
        int userInputOutside = UserInput();
        if(userInputOutside == 0) // End Game
        {
            gameActive = false;
        }
        else if(userInputOutside == 1) //Start Game - new decks
        {
            Array.Resize(ref playerDeck, 2);
            Array.Resize(ref dealerDeck, 1);
            playerDeck = DeckBuilder(playerDeck);
            dealerDeck = DeckBuilder(dealerDeck);
            gameOver = false;
            canChangeBet = true;
        }
    else if(userInputOutside == 5){Help();} // Help Menu
    else if(userInputOutside == 4 ){bet = BetSystem(balance, bet, false);}// Change bet
    
    }
    
    
}

static int[] DealerDraw(int[] dealerDeck)
{
    string[] dealDeckArray = DeckToStringArray(dealerDeck);
    for(int i = 0; dealerDeck.Sum() < 17; i++)
            {
                Thread.Sleep(1000);
                System.Console.Write("Dealer is drawing card.");
                for(int j = 0; j < 3; j++)
                {
                Thread.Sleep(600);
                System.Console.Write(".");
                }
                dealerDeck = DrawCard(dealerDeck);
                dealDeckArray = DeckToStringArray(dealerDeck);
                System.Console.WriteLine($"\n\nThe dealer drew: {dealDeckArray[i+1]}");
                DeckDisplay(dealerDeck, "Dealer");
                System.Console.Write("\n");

            }
        return dealerDeck;
}

static void DeckDisplay(int[] deck, string deckType)
{
    Console.WriteLine($"{deckType} deck: {string.Join(", ",DeckToStringArray(deck))} (Total = {deck.Sum()})");
}

static string PickAFace()
{
    Random rng = new Random();
    int cardChoice = rng.Next(0,3);
    string outputCard = "Null";
    if(cardChoice == 0)
    {
        outputCard = "Joker";
        return outputCard;
    }
    if(cardChoice == 1)
    {
        outputCard = "Queen";
        return outputCard;
    }
    if(cardChoice == 2)
    {
        outputCard = "King";
        return outputCard;
    }
    return outputCard;
}

static string[] DeckToStringArray(int[] deck)
{
    string[] stringDeck = new string[deck.Length];
    for(int i =0; i < stringDeck.Length; i++)
    {
        if(deck[i] == 11 || deck[i] == 1)
        {
            stringDeck[i] = "Ace";
        }
        else if(deck[i] == 10)
        {
            stringDeck[i] = PickAFace();
        }
        else if(!(deck[i] == 11) || !(deck[i] == 1) || !(deck[i] == 10))
        {
            stringDeck[i] = Convert.ToString(deck[i]);
        }
    }
    return stringDeck;
}

static int[] DeckBuilder(int[] deck)
{
    Random rng = new Random();
    for(int i = 0; i < deck.Length; i++)
        {
            deck[i] = rng.Next(1, 4); 
            if(deck[i] == 1)
                {
                    deck[i] = 11;
                }
            if(deck.Sum() > 21 && deck[i] == 11)
                {
                    deck[i] = 1;
                }
            
        }
        return deck;
}

static void Help()
{
    bool helpMenu = true;
    while(helpMenu){
    System.Console.WriteLine("5 - Help");
    System.Console.WriteLine("4 - Change Bet");
    System.Console.WriteLine("3 - Double");
    System.Console.WriteLine("2 - Stand");
    System.Console.WriteLine("1 - Draw / Begin Game");
    System.Console.WriteLine("0 - Exit");
    System.Console.Write("\nTo exit this menu, enter 5: ");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        if (Console.ReadLine().Equals("5"))
    {
        System.Console.WriteLine("\n---------------------------------------------\n");
        helpMenu = false;
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

}

static int UserInput()
{
    Console.Write($"Input: ");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    string userInput = Console.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
    if (userInput == "" || !userInput.All(char.IsDigit) || Convert.ToInt32(userInput) > 5 || Convert.ToInt32(userInput) < 0)
    {
        Console.WriteLine("Invalid input, for help, enter 5.");
        userInput = "-1";
    }
#pragma warning restore CS8604 // Possible null reference argument.
    System.Console.WriteLine("\n---------------------------------------------\n");
    return Convert.ToInt32(userInput);
}

static decimal BetSystem(decimal balance, decimal currentBet, bool firstBet)
{
    decimal value;
    System.Console.WriteLine($"Current Balance: {balance:C}\n");

    if(firstBet){System.Console.Write("Initial Bet: $");}
    else if(!firstBet)
    {
        System.Console.WriteLine($"Current Bet: {currentBet:C}\n");
        System.Console.Write("New bet: $");
    }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    string betInput = Console.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    while (!decimal.TryParse(betInput, NumberStyles.Float, CultureInfo.InvariantCulture, out value) || Convert.ToDecimal(betInput) < 0.01m || Convert.ToDecimal(betInput) > balance)
    {
        System.Console.Write("Please enter a VALID bet: $");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        betInput = Console.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
    System.Console.WriteLine("\n---------------------------------------------\n");
    return Math.Round(Convert.ToDecimal(betInput), 2);
    
}

static int[] DrawCard(int[] deck)
{
    Random rng = new Random();
    Array.Resize(ref deck, deck.Length + 1);
    deck[deck.Length - 1] = rng.Next(1, 11);
    
    for(int i = 0; i < deck.Length; i++)
    {
        if(deck.Sum() > 21 && deck[i] == 11)
        {
            deck[i] = 1;
        }
    }
    
    return deck;

}

static decimal CardCheck(int[] playerDeck, int[] dealerDeck, decimal bet)
{
    int differencePlayer = Math.Abs(21 - playerDeck.Sum());
    int differenceDealer = Math.Abs(21 - dealerDeck.Sum());
    if(playerDeck.Sum() == 21 && playerDeck.Sum() != dealerDeck.Sum())
    {
        return bet*(decimal)1.5;
    }
    else if(differencePlayer < differenceDealer && playerDeck.Sum() < 21 || dealerDeck.Sum() > 21)
    {

        return bet;
    }
    else if(differencePlayer == differenceDealer)
    {
        return 0;
    }
    else
    {
        return -bet;
    }
    
    
}

static void OutputMessage(int[] playerDeck, int[] dealerDeck)
{
    int differencePlayer = Math.Abs(21 - playerDeck.Sum());
    int differenceDealer = Math.Abs(21 - dealerDeck.Sum());

    if(playerDeck.Sum() == 21 && playerDeck.Sum() != dealerDeck.Sum())
    {
        System.Console.WriteLine("You Won Blackjack!\n");
    }
    else if(differencePlayer < differenceDealer && playerDeck.Sum() < 21 || dealerDeck.Sum() > 21)
    {

        System.Console.WriteLine("You Won!\n");
    }
    else if(differencePlayer == differenceDealer)
    {
        System.Console.WriteLine("You tied\n");
    }
    else
    {
        System.Console.WriteLine("You lost...\n");
    }
    
    DeckDisplay(playerDeck, "Your Final");
    DeckDisplay(dealerDeck, "Dealer's Final");
    System.Console.Write("\n");

}