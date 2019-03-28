package main

import (
	"fmt"
	"strings"

	"github.com/bwmarrin/discordgo"
)

var (
	commandPrefix string
	botID         string
)

func main() {
	/*
		Creating discord session. Associating myself with the bot.
	*/
	discord, err := discordgo.New("Bot NTYwODUzMDQxMDU4Njc2Nzcy.D36FEw.y0K7W_ZII_qasEp5Jhrgbr-uDjM")
	errCheck("error creating discord session", err)
	user, err := discord.User("@me")
	errCheck("error retrieving account", err)

	/*
		Assign bot ID and making handlers for both commands and initial handling
	*/
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

	/*
		Open connection to Discord.
	*/
	err = discord.Open()
	errCheck("Error opening connection to Discord", err)
	defer discord.Close()

	commandPrefix = "!"

	<-make(chan struct{})

}

/*
Checks if an error occured. If so, print the passed in message and abort.
*/
func errCheck(msg string, err error) {
	if err != nil {
		fmt.Printf("%s: %+v", msg, err)
		panic(err)
	}
}

/*
Handle various commands passed into the bot.
discord: the discord session.
message: the command.
*/
func commandHandler(discord *discordgo.Session, message *discordgo.MessageCreate) {
	user := message.Author
	if user.ID == botID || user.Bot {
		//Do nothing because the bot is talking
		return
	}

	content := message.Content

	// Separate command and args if needed.
	command := strings.Split(content, " ")

	comm := command[0]
	args := command[1:]

	switch comm {
	//Simple test command ping. Bot responds with Pong.
	case "!ping":
		discord.ChannelMessageSend(message.ChannelID, "Pong!")
	case "!clear":
		// Clears up to 100 messages less than 2 weeks old in the called channel.
		ch, err := discord.Channel(message.ChannelID)
		errCheck("Oh no! Could not get channel ID.", err)
		msgs, err := discord.ChannelMessages(ch.ID, 100, "", "", "")
		errCheck("Oh no! Could not retrieve messages in channel.", err)
		for _, v := range msgs {
			fmt.Printf("Deleting message %s from user %s\n", v.Content, v.Author)
			discord.ChannelMessageDelete(ch.ID, v.ID)
		}
	case "!listen":
		// Listen to music! (TODO)
		if len(args) == 1 {
			discord.ChannelMessageSend(message.ChannelID, "Proper usage... watch this space.")
			vc, err := discord.ChannelVoiceJoin("560859297278328844", "560880394262806528", false, false)
			errCheck("Couldn't join voice server", err)
			vc.AddHandler(func(vc *discordgo.VoiceConnection, vs *discordgo.VoiceSpeakingUpdate) {

			})

			vc.Disconnect()
			vc.Close()
		} else {
			discord.ChannelMessageSend(message.ChannelID, "Usage: !listen <url>")
		}

	}

	// Debugging server side, prints stuff
	fmt.Printf("Message: %+v || From: %s\n", message.Message, message.Author)
}
