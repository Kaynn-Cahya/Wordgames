module MainMenu
    open System

    type InputMode =
        | Quit = 0
        | Play = 1
        | Help = 2
        | None = 3
    
    let processHelpInput() : unit = 
        Console.WriteLine("You will be given a selection of letters (in yellow) in random order.\r\n Try to form up a word or phrase with those letters!")
        Console.ForegroundColor <- ConsoleColor.Yellow
        Console.WriteLine("NOTE: whitespace in phrases are represented in dashes as so '-'.")
        Console.ForegroundColor <- ConsoleColor.White
    
    let processNoneInput() : unit =
        Console.WriteLine("Type into the console what would you like to do. (Play, Quit, Help)")
        
    let processInput (input: InputMode) : unit = 
        if input = InputMode.Help then
            processHelpInput()
        elif input = InputMode.Play then
            Wordgame.play()
            processNoneInput()
        else
            processNoneInput()
    
    let getInputMode (inputArg: string) : InputMode =
        let inputUpper = inputArg.Trim().ToUpper()
    
        if inputUpper = "QUIT" then
            InputMode.Quit
        elif inputUpper = "HELP" then
            InputMode.Help
        elif inputUpper = "PLAY" then
            InputMode.Play
        else
            InputMode.None
