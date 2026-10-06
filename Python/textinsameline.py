import time

def message(text):
    time.sleep(2)
    print(f'\r{text} \033[K', end=" ")
   

message("Hello, Peter - this is an example. This is a very long message my")

message('Hello, this is 2nd message')

message('hello this is short')

message('short')
