#!/bin/bash
clear
tput civis

chars=("ｱ" "ｲ" "ｳ" "ｴ" "ｵ" "ｶ" "ｷ" "ｸ" "ｹ" "ｺ" "ｻ" "ｼ" "ｽ" "ｾ" "ｿ" "0" "1" "2" "3" "4" "5" "6" "7" "8" "9" "A" "B" "C" "D" "E" "F" "G" "H" "I" "J" "K" "L" "M" "N" "O" "P" "Q" "R" "S" "T" "U" "V" "W" "X" "Y" "Z")

rows=$(tput lines)
cols=$(tput cols)

while true; do
    col=$((RANDOM % cols))  # Pick a random column
    char=${chars[$((RANDOM % ${#chars[@]}))]}  # Pick a random character
    tput cup 0 $col  # Move cursor to the top of the column
    echo -ne "\033[32m$char\033[0m"  # Print the character in green

    for ((i=1; i<rows; i++)); do
        tput cup $i $col
        echo -ne "\033[32m$char\033[0m"
        sleep 0.1
    done
done

