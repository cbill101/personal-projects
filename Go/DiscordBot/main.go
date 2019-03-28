package main

import (
	"fmt"

	"github.com/bwmarrin/discordgo"
)

var (
	commandPrefix string
	botID         string
)

func main() {
	discord, err := discordgo.New("Bot NTYwODUzMDQxMDU4Njc2Nzcy.D36FEw.y0K7W_ZII_qasEp5Jhrgbr-uDjM")
	errCheck("error creating discord session", err)
	user, err := discord.User("@me")
	errCheck("error retrieving account", err)

	botID = user.ID
	discord.AddHandler(commandHandler)
	discord.AddHandler(func(discord *discordgo.Session, ready *discordgo.Ready) {
		err = discord.UpdateStatus(0, "With your mind... and learning Go.")
		if err != nil {
			fmt.Println("Error attempting to set my status")
		}
		servers := discord.State.Guilds
		fmt.Printf("Testing bot has started on %d servers", len(servers))
	})

	err = discord.Open()
	errCheck("Error opening connection to Discord", err)
	defer discord.Close()

	commandPrefix = "!"

	<-make(chan struct{})

}

func errCheck(msg string, err error) {
	if err != nil {
		fmt.Printf("%s: %+v", msg, err)
		panic(err)
	}
}

func commandHandler(discord *discordgo.Session, message *discordgo.MessageCreate) {
	user := message.Author
	if user.ID == botID || user.Bot {
		//Do nothing because the bot is talking
		return
	}

	content := message.Content

	switch content {
	case "!ping":
		discord.ChannelMessageSend(message.ChannelID, "Pong!")
	case "!clear":
		ch, err := discord.Channel(message.ChannelID)
		errCheck("Oh no! Could not get channel ID.", err)
		msgs, err := discord.ChannelMessages(ch.ID, 100, "", "", "")
		errCheck("Oh no! Could not retrieve messages in channel.", err)
		for _, v := range msgs {
			discord.ChannelMessageDelete(ch.ID, v.ID)
		}
	}

	fmt.Printf("Message: %+v || Content: %+v || From: %s\n", message.Message, content, message.Author)
}
