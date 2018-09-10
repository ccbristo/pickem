#!/usr/bin/env python3
import os
import subprocess
import time

SLEEP_SECONDS = 300

print("----------------------------------------")
print("  Pick'em game updating looper")
print("----------------------------------------")

while(True):
    if (os.name == 'nt'):
        # windoze no ./
        subprocess.call("pickem-game-syncher.py -ns 2018 -ps 18 -w 3 -a update", shell=True)
    else:
        #nix
        subprocess.call("./pickem-game-syncher.py -ns 2018 -ps 18 -w 3 -a update", shell=True)
        
    print("-- snoozing " + str(SLEEP_SECONDS) + " seconds")
    time.sleep(SLEEP_SECONDS)

