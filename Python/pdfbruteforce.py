import pypdf
from pypdf import PdfReader
import pypdf.annotations
import pypdf.errors

pdfFile = "/home/kali/Desktop/python/test.pdf"
dicFile = open("/home/kali/Desktop/python/wordlists/fasttrack.txt", "r")
readDicLines = dicFile.readlines()

def label():
    print("""
        PDF Bruteforce
          
          
          """)


def bruteforce():
    for i in readDicLines:
        i = i.strip()
        pdfOpen = open(pdfFile, "rb")
        pdfRead = PdfReader(pdfOpen)
        print("trying password ", i)
        u_case = pdfRead.decrypt(i)
        if u_case != 0:
            print("\n\npassword is", i)
            break
        l_case = pdfRead.decrypt(i)
        if l_case != 0:
            print("lowercase oassword is", i)
            break
        else:
            print("password not found")

        pdfOpen.close()

def main():
    label()
    options = input("""Please select from the following:
                    1. PDF Bruteforce
                    99. Exit 
                    
                    Your selection: """)
    if options == "1":
        bruteforce()
    elif options == "99":
        exit()
    else:
        print("Incorrect option")

main()
