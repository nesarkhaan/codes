#!/bin/python3

import subprocess

#settings

debug = False

#lists 
identified_hosts=[]

class textColors:
    Black = 30
    Red = 31
    Green = 32
    Yellow = 33
    Blue = 34
    Purple = 35
    Cyan = 36
    White = 37

class textStyle:
    noeffect = 0
    Bold = 1
    Underline = 2
    Negative1 = 3
    Negative2 = 5

class textBackground:
    Black = 40
    Red = 41
    Green = 42
    Yellow = 43
    Blue = 44
    Purple = 45
    Cyan = 46
    White = 47

## Functions

# creating a text print function in order to semi automate the color rendering of the text. \333[K or end='' can be used to render the back to th entire line
# textPrint function can be used with different variables.

def textPrint(textfront, textback, textdesign, text):
    string = f'\033[{textdesign};{textfront};{textback}m {text} \033[{textback}m \033[K \033[{textStyle.noeffect};{textColors.Blue};{textBackground.White}m' #end='')
    print(string.center(24, " "))


#creating a textInput function to automate color rendering.

def textInput(textfront, textback, textdesign, text):
    string = f'\033[{textdesign};{textfront};{textback}m {text} \033[{textback}m \033[K'
    string_input = input(string)
    return string_input

# A function for printing outputs if debug is set to True. 
def printdebug(text):
    if debug == True:
        textPrint(textColors.White, textBackground.Red, textStyle.Bold, "DEBUG:")
        textPrint(textColors.White, textBackground.Red, textStyle.Bold, text)

# Function splits the network IP / Cidr. If network is 10.0.0.0/16 then this splits it into '10.0.0.0' and '16'

def splitNetwork(network_ip):
    network_split = network_ip.split("/")
    if debug == True: print(f'\nThe network ip range has been split as follows: {network_split}\n') 
    return network_split

# Function that calculates the number of hosts avialable from the cidr.

def calcHosts(network_ip):
    # Taking the return daa from splitNetwork functions and placing it in a list variable.
    network_split = splitNetwork(network_ip) 
    # Converting list item 1(Cidr string - i.e 16) into integer.
    network_int = int(network_split[1])
    printdebug(f'Network Split has nominated - {network_int} and converted it to an integer')
    # Using the formula to calculate number of hosts. We subtract the cidr number from 32 - IPv4.
    addresses = 32 - network_int
    printdebug(f'Since IPv4 Networks are 32 bits, we will subtract the network cidr from 32 which results in {addresses}')
    # The result of the subtraction is hen added to the 2 to the power. I.e 32 - 16 = 16 therefore it would be 2 to the power or 16
    calc = 2
    printdebug(f'calc variable has been set to 2 so we can multiply it within the loop. Current Value is {calc}\n')

    printdebug(f'Commencing loop to calculate the number of addresses')
    #This loop then multiples calc value 2 with 2. The calc value changes with each multiplication until the number of addresses is reached i.e loops
    for i in range(1, addresses):
        calc = calc * 2
        printdebug(f'Round {i} of loop. Result {calc}')
    print(f'there are {calc} hosts addresses in CIDR {network_int}')
    return calc

def idHosts_ping(network_ip, hosts):

    Get_IP = splitNetwork(network_ip)
    Get_IP = Get_IP[0]
    printdebug(f'This is the get IP {Get_IP}')
    Split_IP = Get_IP.split('.')
    printdebug(f'This is the get IP after split {Split_IP}')

    host_list = []
    
    #Commence the loop to creating IP addresses
    host_ID_1 = Split_IP[0]
    host_ID_2 = Split_IP[1]
    host_ID_3 = Split_IP[2] 
    host_ID_4 = Split_IP[3]
    host_ID = 0
    for i in range(1, hosts + 1):
        host_ID = host_ID + int(host_ID_4) + 1
        addresses = f'{host_ID_1}.{host_ID_2}.{host_ID_3}.{host_ID}'
        host_list.append(addresses)
        printdebug(text=f"Printing the addresses for Host_list, {addresses}")
        if debug == True: print(host_list)
        
    #ping
    textPrint(textColors.Blue, textBackground.White, textStyle.Bold, "Ping commenced. This may take some time...")

    for i in host_list:
        command = f'ping {i} -c 4'
        printdebug(f'Running command - {command}')
        #Running the ping command. Shell is true since it will be run in shel. Capture output is true as we want to capture the end output result. 
        ping = subprocess.run(command, shell=True, capture_output=True, text=True)
        #print(ping.stdout)

        lines = ping.stdout
        print(ping.returncode)
        
        if ping.returncode == 0:
            identified_hosts.append(i)
            printdebug(f'List of identified hosts {identified_hosts}')
        else:
            continue
        
        #if '4 packets transmitted, 4 received' in lines:
        #    line = lines.splitlines()
        #    identified_hosts.append(i)
        #    printdebug(f'List of identified hosts {identified_hosts}')
            
        #elif 'Host unreachable' in lines:
        #   continue
        
    print('The following hosts were identified', identified_hosts)
    return identified_hosts


##########################################################################################

def main():
    #Printing the title
    textPrint(textColors.Blue, textBackground.White, textStyle.Bold, "Welcome to the Port scanner script. This title will be updated in due time")


    network_ip = "10.0.2.0/28" #textInput(textColors.Blue, textBackground.Purple, textStyle.Underline&textStyle.Bold, 'Please enter an output file.')
    #print('This is printing the string_input', network_ip)

    #The calchost function calculated the number of hosts available. Host numbers placed in variable hosts.
    hosts = calcHosts(network_ip)

    #the idHosts_ping splits the IP based on the [.], then generates hosts followed by #ping. More info in function. 
    ping_results = idHosts_ping(network_ip, hosts)


main()