module Wordgame   

    open System
    open System.IO
    open System.Reflection
    open System.Text.RegularExpressions
    open System.Collections.Generic
    open System.Linq

    let private FilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Assets", "words.txt")

    let private askContinueGame() : bool =
        let regExp = new Regex("^(?i:Yes|No|Y|N)$")
        
        let mutable input = String.Empty
        while not (regExp.IsMatch(input)) do
            Console.WriteLine("Do you want to continue playing? (Y/N)")
            input <- Console.ReadLine()

        if Regex.Match(input, "^(?i:Yes|Y)$").Success then
            true
        else
            false

    let private getRandomWord() : string =
        use streamReader = new StreamReader(FilePath)

        let mutable choosen = String.Empty;
        let mutable linesNotSeen = File.ReadLines(FilePath).Count()
        let random = new Random()

        let mutable currLine = streamReader.ReadLine()
        let mutable wordNotChoosen = true
        while not (currLine = null) && wordNotChoosen do
            linesNotSeen <- linesNotSeen - 1
            if random.Next(0, linesNotSeen) = 0 then
                choosen <- currLine
                wordNotChoosen <- false
            else
                currLine <- streamReader.ReadLine()

        choosen.Trim()

    let private showScrambledChars (input : char array) : unit =
        Console.Write("You have these characters to use from\r\n(You need not use all):\r\n")
        Console.ForegroundColor <- ConsoleColor.Yellow
        for c in input do
            Console.Write(" " + c.ToString() + " ")

        Console.Write("\r\n")
        Console.ForegroundColor <- ConsoleColor.White

    let private scrambleChars (input : string) : char array =
        let rand = new Random()

        let result = new SortedList<int, char>()
        for c in input do
            result.Add(rand.Next(), c)

        result.Values.ToArray()

    // Checks if the user inputted some extra character that is not given
    let private isValidAnswer(userAnswer : string, correctAnswer : string) : bool =
        let usersInput = new Dictionary<char, int>()
        let answer = new Dictionary<char, int>()
        
        for c in userAnswer do
            if usersInput.ContainsKey(c) then
                usersInput.[c] <- usersInput.[c] + 1
            else
                usersInput.Add(c, 1)

        for c in correctAnswer do
            if answer.ContainsKey(c) then
                answer.[c] <- answer.[c] + 1
            else
                answer.Add(c, 1)

        let mutable isValid = true
        for cCount in usersInput do
            if answer.ContainsKey(cCount.Key) && isValid then
                if answer.[cCount.Key] < cCount.Value then
                    isValid <- false
                else
                    isValid <- true
            else
                isValid <- false

        isValid

    let private processAnswer (userAnswer : string, correctAnswer : string) : bool =
        if userAnswer.ToUpper().Equals("GIVEUP") then
            Console.WriteLine("One valid english word you could have used was '" + correctAnswer + "'.")
            true
        elif not (isValidAnswer(userAnswer.Trim(), correctAnswer)) then
            Console.ForegroundColor <- ConsoleColor.DarkRed
            Console.WriteLine("You used some additional characters that were not provided!\r\n(Take note of case sensitivity!)\r\n")
            Console.ForegroundColor <- ConsoleColor.White
            false
        elif String.IsNullOrWhiteSpace(userAnswer) then
            Console.ForegroundColor <- ConsoleColor.DarkRed
            Console.WriteLine("You just gave an empty input!\r\n")
            Console.ForegroundColor <- ConsoleColor.White
            false
        else
            use reader = File.OpenText(FilePath)
            let allTheWords = reader.ReadToEnd()
            let regExp = new Regex(userAnswer)

            if regExp.IsMatch(userAnswer) then
                Console.WriteLine("Your answer is valid english word!")
                true
            else
                Console.WriteLine("Your answer is not valid english word!")
                false
        

    let public play() : unit =
        Console.ForegroundColor <- ConsoleColor.Cyan
        Console.WriteLine("\r\nStarting a game, use 'giveup' if you think you can't solve it.\r\nNOTE: Whitespaces are defined as dashes, like so '-'.")
        Console.ForegroundColor <- ConsoleColor.White

        let mutable continueGame = true
        while continueGame do
            // Get a random word that we ideally want the user to guess.
            // (But the user can guess another word, if they can form it up with the given letters)
            let wordToReference = getRandomWord()

            let scrambledChars = scrambleChars(wordToReference)
            Console.Write("\r\n")

            showScrambledChars(scrambledChars)

            let mutable userAnswer = Console.ReadLine()

            while not (processAnswer(userAnswer, wordToReference)) do
                showScrambledChars(scrambledChars)
                userAnswer <- Console.ReadLine()
                Console.Write("\r\n")

            continueGame <- askContinueGame()