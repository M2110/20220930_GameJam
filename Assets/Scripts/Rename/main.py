import os

folder = "D:\\Dokumente\\Entwicklung\\Unity\\20220930_GameJam\\Assets\\Characters\\png\\hurt\\Animations"
prefix = "hurt"

files = os.listdir(folder)

for file in files:
    os.rename(f"{folder}\\{file}", f"{folder}\\{prefix}_{file}")
