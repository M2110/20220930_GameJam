import os

folder = "C:\\Users\\juliu\\Documents\\GameJam\\20220930_GameJam\\Assets\\Characters\\png\\walkcycle\\Controller"
prefix = "walkcycle"

files = os.listdir(folder)

for file in files:
    os.rename(f"{folder}\\{file}", f"{folder}\\{prefix}_{file}")
