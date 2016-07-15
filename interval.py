# eventSchema = {
# 	eType = [noteOn, noteOff, single]
# }
import random

def createEvents(numEvents):
	songLengthMin = 5.0
	events = []
	for _ in range(numEvents):
		timeStart = random.random() * 60 * songLengthMin 
		timeEnd = timeStart + (songLengthMin - timeStart) * random.random()

		endEvent = {
			"noteOn": False,
			"time": timeEnd
		}

		startEvent = {
			"noteOn": True,
			"time": timeStart
		}  

		endEvent["startEventRef"] = startEvent
		startEvent["endEventRef"] = endEvent

		events.append(startEvent)
		events.append(endEvent)
	return events

def sortEvents(events):
	return sorted(events, key=lambda e: e["time"])

def findAfter(events, time, headStartIndex = 0):
	for i in range(headStartIndex, len(events)):
		event = events[i]
		if event["time"] > time:
			return i

def findBefore(events, time, headStartIndex = 0):
	for i in range(headStartIndex, len(events)):
		event = events[i]
		if event["time"] > time:
			return max(i - 1, 0)
# def findBefore(events, time):


events = createEvents(50)
sortedEvents =  sortEvents(events)
for e in sortedEvents:
	print e["time"]


windowLengthSec = 20.0
startIndex = 0
endIndex = 0




def onEnter(event):
	print "Enter", event

def onExit(event):
	print "Exit", event	



def moveForward(events, time, window):
	global startIndex
	global endIndex
	newStartIndex = findAfter(events, time, startIndex)
	newEndIndex = findBefore(events, time + window, endIndex)

	
	for i in range(startIndex, newStartIndex):
		#indexes that are no longer visible
		onExit(events[i])
	for i in range(endIndex+1, newEndIndex+1):
		#indexes that just became visible
		onEnter(events[i]) 

	startIndex = newStartIndex
	endIndex = newEndIndex

import time

startTime = time.time()
while True:
	relTime = time.time() - startTime 
	# print relTime
	moveForward(sortedEvents, relTime, windowLengthSec)










