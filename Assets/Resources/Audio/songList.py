import glob
import os
import json

fileExtensions = ["mp3", "wav", "ogg"]



def main():
	fileNames = getFileNames()
	print fileNames

	json = convertToJson(fileNames)
	print json


	with open("songList.json", "w") as outfile:
		outfile.write(json)


def getFileNames(stripExt = True):
	names = []

	for ext in fileExtensions:
	#all file names with that extension
		fileNames = glob.glob("*." + ext)
		for fileName in fileNames:
			if stripExt:
				(name, ext) = os.path.splitext(fileName)
				names.append(name)
			else:
				names.append(fileName)
	return names

def convertToJson(fileNames):

	preJson = {}

	for fileName in fileNames:
		meta = {
			"enabled": True
		}

		preJson[fileName] = meta
	# print(preJson

	return json.dumps(preJson)	

main()







