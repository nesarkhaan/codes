#!/usr/bin/env python3

# the subprocess module is required to run commands
import subprocess

# This supplies the ipaddress module will be used to validate the IP address
import ipaddress

#Defining function start. The function contains the logo and the text for starting program.
def start ():
    start_art = """.----------------.  .----------------.  .----------------.  .----------------.  .----------------. 
    | .--------------. || .--------------. || .--------------. || .--------------. || .--------------. |
    | |    ______    | || |  _________   | || |   _____      | || |     ____     | || |    _______   | |
    | |  .' ___  |   | || | |_   ___  |  | || |  |_   _|     | || |   .'    `.   | || |   /  ___  |  | |
    | | / .'   \_|   | || |   | |_  \_|  | || |    | |       | || |  /  .--.  \  | || |  |  (__ \_|  | |
    | | | |    ____  | || |   |  _|  _   | || |    | |   _   | || |  | |    | |  | || |   '.___`-.   | |
    | | \ `.___]  _| | || |  _| |___/ |  | || |   _| |__/ |  | || |  \  `--'  /  | || |  |`\____) |  | |
    | |  `._____.'   | || | |_________|  | || |  |________|  | || |   `.____.'   | || |  |_______.'  | |
    | |              | || |              | || |              | || |              | || |              | |
    | '--------------' || '--------------' || '--------------' || '--------------' || '--------------' |
     '----------------'  '----------------'  '----------------'  '----------------'  '----------------' 
    """
    print(start_art)
    print("Starting program...")
    print("""
    This Network Scanner script is a tool specifically designed for Gelos Enterprise to perform automated network 
    reconnaissance by scanning a specified IP range for open ports and identifying hosts with the open port 80/tcp. 
    It utilises the Nmap tool for port scanning and the Gobuster tool for directory scanning on identified hosts.
    
    Dependencies:
    - Python 3.x
    - Nmap
    - Gobuster 
    - Wordlist - /usr/share/wordlists/dirbuster/directory-list-2.3-medium.txt

    Author: Nes Kakar - Email: nesar.kakar5@studytafensw.edu.au
    Version: 1.0
    Date: 29/03/2024
    
    Usage:
    1. Run the script and provide the IP range for the target network.
    2. Specify the output file name to save the scan results.
    3. The script performs the Nmap scan and identifies hosts with open port 80/tcp.
    4. For each identified host, it performs a Gobuster directory scan.
    5. The results are saved to a text file for further analysis.
    """)


# Defining a function to validate IP address.
def ip_address_validation(network_ip):
    # Start an infinite loop to continuously validate the IP address until it is valid.
    while True:
        try:
            ip_obj = ipaddress.ip_network(network_ip, strict=False) # Validating whether provided IP address is valid or not.

            # Check if the subnet mask '/' is missing in the IP address.

            if '/' not in network_ip:
                # Raise a ValueError if the subnet mask is missing, providing an error message.
                raise ValueError(f"""
            ERROR: Network address {network_ip} is incorrect as it is missing CIDR notation
            Please enter as follows. For example: {network_ip}/24
                
                    """)
            # If no exception is raised, return the validated network IP address.
            return network_ip
        except ValueError as e:
            # Catch any ValueError exceptions raised during IP address validation.
            print(f"ERROR:{e}")

            # Prompt the user for further action if the IP address is invalid.
            options = input("""Please select from the following options:
            1. Re Enter another IP address
            2. End the Program
            
            Your selection: """)

            # Check the user's choice and proceed accordingly.
            if options == '1':
                # If the user chooses to re-enter another IP address, prompt for input.
                network_ip = input("""Please re enter another IP address:
                 IP Address: """)
            elif options == '2':
                # If the user chooses to end the program, exit.
                exit()
            else:
                # If an invalid choice is entered, inform the user and continue the loop.
                print("Invalid choice. Please enter 1 or 2")

# Define a function to perform an Nmap scan on the specified network IP address.
def nmap_scan(network_ip):
    # Print a message indicating that the scanning process is starting for the specified network IP.
    print(f"Scanning {network_ip}")

    # Construct the Nmap command with the '--open' and '-v' options for scanning open ports and providing verbose output.
    command = f"nmap --open -v {network_ip}"

    #print("NMAP Command:   ", command) # The nmap command used. For testing only.

    # Run the Nmap command in the shell, capturing the output and ensuring it's returned as text.
    result = subprocess.run(command, shell=True, capture_output=True, text=True)

    # Print a message indicating that the scan is complete.
    print(f"Scan complete")

    # Return the standard output (stdout) of the Nmap command, which contains the scan results.
    return result.stdout
def save_network_scan(network_ip, nmap_result, results_file ):
    # Replace '/' in the network IP with '-' to sanitize it for the filename
    sanitise_network_ip = network_ip.replace('/', '-')

    # Call save_result_to_file function to save the network scan results
    save_result_to_file(f"#####################  Network Scan for {network_ip}  ##################### \n \n {nmap_result} ", f"Network-{sanitise_network_ip}_{results_file}-scan.txt")
def extract_identified_hosts(nmap_result, network_ip): # starting a function which reads the nmap result and identifies hosts with open port 80

    # Print message indicating identification of open ports
    print(f"Attempting to Identify hosts with open port 80")

    # Split the nmap result string into lines
    lines = nmap_result.split('\n')

    # Initialise an empty list to store IP addresses of hosts with open port 80
    hosts = []

    # Iterate through each line in the nmap result
    for line in lines:
        # Check if the line contains information about an open port 80
        if "Discovered open port 80/tcp" in line:

            # Split the line into parts
            parts = line.split()

            # Extract the IP address from the line
            host_ip = parts[5]

            # Append the IP address to the hosts list
            hosts.append(host_ip)

            # Print message indicating identification of a host with open port 80
            print(f"Identified host {host_ip} with open port 80 ")

    # Check if no hosts with open ports were identified
    if not hosts:
        # Prompt the user for options if no hosts were identified
        print(f"\nI am sorry but no hosts with open ports were identified for network {network_ip}")
        options = input("""Please select from the following options: \n
                   1. Re Enter another IP address
                   2. End the Program

                   Your selection: """)

        # If the user chooses to re-enter another IP address
        if options == '1':
           return False # Return False to indicate no hosts were identified for the while loop in main()
        # If the user chooses to end the program
        elif options == '2':
           exit() # Exit the program
           # If the user enters an invalid choice
        else:
           print("Invalid choice. Please enter 1 or 2")
    return hosts # Return the list of identified hosts
def gobuster_scan(hosts, nmap_result, network_ip, results_file): #Starting a function for gobuster scan. It takes the results hosts list from extract_identified_host function.
    # Iterate through each identified host with open port 80
    for host in hosts:
        # Print message indicating the start of directory scan for the host
        print(f"Performing directory scan on http://{host}:80")

        # Define the command for gobuster directory scan.
        command = f"gobuster dir -u http://{host}:80 -w /usr/share/wordlists/dirbuster/directory-list-2.3-medium.txt"

        # Execute the command and capture the output
        result = subprocess.run(command, shell=True, capture_output=True, text=True)

        # Replace '/' in the network IP to sanitize it for file naming
        sanitise_network_ip = network_ip.replace('/', '-')

        # Save the directory scan result to the output file
        save_result_to_file(f"\n \n \n #####################  Directory Scan for {host}  #####################\n \n{result.stdout}", f"Network-{sanitise_network_ip}_{results_file}-scan.txt")

        # Print message indicating completion of directory scan for the host
        print(f"Complete.")
#Defining function to save file.
def save_result_to_file(data, filename):
    # Open the file in 'append' mode and write the data to it
    with open(filename, 'a') as file:
        file.write(data)

def saved_file(network_ip, results_file):
    # Replace '/' in the network IP with '-' to sanitize the IP
    sanitise_network_ip = network_ip.replace('/', '-')

    # Print a message indicating where the results are saved
    print(f"Results saved to File - Network-{sanitise_network_ip}_{results_file}-scan.txt")

# Defining function main() containing all the different steps to run the program.
def main():

    # Step 1: Start the program and display initial message
    start()

    # Step 2: Continuous loop to handle multiple scans
    while True:

        # Step 3: Prompt user for IP range and output file name
        network_ip = input("Enter IP range for target: ")
        network_ip = ip_address_validation(network_ip)
        results_file = input("Enter output file name: ")

        # Step 4: Perform Nmap scan and save results
        nmap_result = nmap_scan(network_ip)
        save_network_scan(network_ip, nmap_result, results_file)

        # Step 5: Extract identified hosts from Nmap scan results
        hosts = extract_identified_hosts(nmap_result, network_ip)
        if hosts is False:  # If no hosts are identified, disregard the next step and continue to the next iteration of the loop at step3.
             continue

        # Step 6: Perform directory scan using Gobuster and save results
        gobuster_scan(hosts, nmap_result, network_ip, results_file)

        # Step 7: Display message indicating where the results are saved
        saved_file(network_ip, results_file)

        # Step 8: Prompt user to enter another IP address or end the program
        choice = input("Do you want to enter another IP address? (yes/no): ").lower()
        if choice != 'yes':
            break # Exit the loop if the user chooses not to enter another IP address

main()