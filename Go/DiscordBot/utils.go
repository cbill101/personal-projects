package main

import (
	"fmt"

	"github.com/bwmarrin/discordgo"
)

/*
errCheck ...
Checks if an error occured. If so, print the passed in message and abort.

msg: the string to print out to the console.
err: the error to print out.
*/
func errCheck(msg string, err error) {
	if err != nil {
		fmt.Printf("%s: %+v", msg, err)
		panic(err)
	}
}

/*
errCheckNonPanic ...
Checks if an error occured. If so, print the passed in message to both console
and Discord channel.

Only use if you don't want to panic after a non fatal error occurs.

msg: the string to print out to the console.
err: the error to print out.
*/
func errCheckNonPanic(discord *discordgo.Session, message *discordgo.MessageCreate, msg string, err error) {
	if err != nil {
		_, err := discord.ChannelMessageSend(message.ChannelID, msg)
		fmt.Printf("%s: %+v", msg, err)
	}
}
