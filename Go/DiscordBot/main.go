package main

import (
	"fmt"
	"strings"

	"github.com/bwmarrin/discordgo"
)

var (
	commandPrefix string
	botID         string
	voiceConn     *discordgo.VoiceConnection
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
		fmt.Printf("Testing bot has started on %d servers\n", len(servers))
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
commandHandler handles various commands passed into the bot.
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
		go PingCommand(discord, message)
	case "!clear":
		// Clears up to 100 messages less than 2 weeks old in the called channel.
		go ClearCommand(discord, message)
	case "!listen":
		// Queries Youtube to get music to listen.
		go ListenCommand(discord, message, args)
	case "!join":
		// Joins voice channel you're currently in.
		go JoinCommand(discord, message)
	case "!leave":
		// Leaves voice channel, if in one.
		go LeaveCommand(discord, message)
	case "!rank":
		// Set's your rank.
		go RankCommand(discord, message, args)
	case "!help":
		// Lists commands and what not
		go HelpCommand(discord, message)
	case "!nick":
		go NickCommand(discord, message, args)
	case "!setnick":
		go SetnickCommand(discord, message, args)
	}

	// Debugging server side, prints stuff
	fmt.Printf("Message: %+v || From: %s\n", message.Message, message.Author)
}
