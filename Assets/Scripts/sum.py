import glob



def sum():
    files = glob.glob("*.cs")
    total = 0
    for f in files:
        for line in open(f):
            total += 1
    print(total)


sum()