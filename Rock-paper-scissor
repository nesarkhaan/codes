import random

game_counter = 0
Wins = 0
loses = 0
Draws = 0


print("""██████████████████████████
█▄─▄▄▀█─▄▄─█─▄▄▄─█▄─█─▄███
██─▄─▄█─██─█─███▀██─▄▀████
▀▄▄▀▄▄▀▄▄▄▄▀▄▄▄▄▄▀▄▄▀▄▄▀▀▀
█████████████████████████████████
█▄─▄▄─██▀▄─██▄─▄▄─█▄─▄▄─█▄─▄▄▀███
██─▄▄▄██─▀─███─▄▄▄██─▄█▀██─▄─▄███
▀▄▄▄▀▀▀▄▄▀▄▄▀▄▄▄▀▀▀▄▄▄▄▄▀▄▄▀▄▄▀▀▀
██████████████████████████████████████████████
█─▄▄▄▄█─▄▄▄─█▄─▄█─▄▄▄▄█─▄▄▄▄█─▄▄─█▄─▄▄▀█─▄▄▄▄█
█▄▄▄▄─█─███▀██─██▄▄▄▄─█▄▄▄▄─█─██─██─▄─▄█▄▄▄▄─█
▀▄▄▄▄▄▀▄▄▄▄▄▀▄▄▄▀▄▄▄▄▄▀▄▄▄▄▄▀▄▄▄▄▀▄▄▀▄▄▀▄▄▄▄▄▀""")





def User_Input(user_input):
    #rocks = rock()
    user_input = input(f'''
    1. Rock  \t \t \t 2. Paper  \t \t \t  3. Scissors 
                       
      Please select your reponse: ''')

    if user_input == "1":
        user_selection = "Rock"
        print(f"You have selected: {user_selection} ({user_input})")
    elif user_input == "2":
        user_selection = "Paper"
        print(f"You have selected: {user_selection} ({user_input})")
    elif user_input == "3":
        user_selection = "Scissors"
        print(f"You have selected: {user_selection} ({user_input})")

    else:
        print("wrong selection")

    return user_input

def Computer_Input():
    randomise = random.randint(1, 3)

    if randomise == 1:
        computer_selection = "Rock"
        print(f"Computer selected: {computer_selection} ({randomise})")

    elif randomise == 2:
        computer_selection = "Paper"
        result = randomise
        print(f"Computer selected: {computer_selection} ({randomise})")

    elif randomise == 3:
        computer_selection = "Scissors"
        print(f"Computer selected: {computer_selection} ({randomise})")
        
    return randomise

while True:
    
    user_input = 0
    computer_input = 0
    user_result = User_Input(user_input)
    Computer_result = Computer_Input()

    if int(user_result) == Computer_result:
        print("It's a draw")
        Draws = Draws + 1
    elif int(user_result) == 1:
        if Computer_result== 2:
            print("You lose! Paper covers rock")
            loses = loses + 1
        else:
            print("You win! Rock smashes scissors") 
            Wins = Wins + 1 
    elif int(user_result) == 2:
        if Computer_result == 1:
            print("You win. Paper covers rock")
            Wins = Wins + 1
        else:
            print("You lose! scissor cuts paper")
            loses = loses + 1
    elif int(user_result) == 3:
        if Computer_result == 2:
            print("You win! Scissor cuts paper")
            Wins = Wins + 1
        else:
            print("You lose! Rock smashes scissors")
            loses = loses + 1

    game_counter = game_counter + 1
    print(f'''Game Statistics: Game Number: {game_counter}   |   Wins: {Wins}    |    Loses: {loses}   |     Draws: {Draws}''')        
          
