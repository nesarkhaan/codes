#password generator
import random

uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
lowercase  = "abcdefghijklmnopqrstuvwxyz"
special_char = "@!%?&$+=#*"

password_limit = 20
pass_list = []

for i in range(password_limit):
    round = random.randint(0, 3)
    if round == 0:
        u_rand = random.randint(0 ,25)
        letter = uppercase[u_rand]
        pass_list.append(letter)
    if round == 1:
        l_rand = random.randint(0 ,25)
        letter = lowercase[l_rand]
        pass_list.append(letter)
    if round == 2:
        char_rand = random.randint(0 ,9)
        letter = special_char[char_rand]
        pass_list.append(letter)

    if round == 3:
        num_rand = random.randint(0 , 9)
        letter = num_rand
        pass_list.append(letter)

print ("Random Password is: ", *pass_list, sep='')
