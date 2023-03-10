using Moq;

namespace SocialNetwork.Tests;

public class TwitterTests
{
    private Mock<Output> _console;
    private Mock<Parser> _parser;
    private Mock<Repository> _repository;
    private Twitter _twitter;

    
    public TwitterTests()
    {
        _console = new Mock<Output>();;
        _parser = new Mock<Parser>();
        _repository = new Mock<Repository>();
        _twitter = new Twitter(_console.Object, _parser.Object, _repository.Object);
        
    }


    [Fact]
    public void ShouldSendCommandToParser()
    {
        var userName = "Alice";
        var message = "I love the weather today";
        var command = $"{userName} -> {message}";
        var parsedCommand = new Command(userName, CommandType.Post, message);
        
        _parser.Setup(x => x.ParseCommand(command)).Returns(parsedCommand);
        _twitter.SendCommand(command);
        
        _parser.Verify(v => v.ParseCommand(command));
    }

    [Fact]
    public void ShouldSavePostCommand()
    {
        var userName = "Alice";
        var message = "I love the weather today";
        var command = $"{userName} -> {message}";
        var parsedCommand = new Command(userName, CommandType.Post, message);
        
        _parser.Setup(x => x.ParseCommand(command)).Returns(parsedCommand);
        _repository.Setup(x => x.SaveMessage(userName, message));
        
        _twitter.SendCommand(command);

        _repository.Verify(v => v.SaveMessage(userName, message));
    }

    [Fact]
    public void ShouldReadMessagesFromAlice()
    {
        var userName = "Alice";
        var parsedCommand = new Command(userName, CommandType.Read);
        
        _parser.Setup(x => x.ParseCommand(userName)).Returns(parsedCommand);
        _repository.Setup(x => x.ReadMessagesFromUser(userName)).Returns(new List<string>());
        
        _twitter.SendCommand(userName);
        
        _repository.Verify(v => v.ReadMessagesFromUser(userName));
    }

    [Fact]
    public void ShouldReadMessages()
    {
        var userName = "Alice";
        var message = "I love the weather today";
        var messagesList = new List<string>();
        messagesList.Add(message);

        _parser.Setup(x => x.ParseCommand(userName)).Returns(new Command(userName, CommandType.Read));
        _repository.Setup(x => x.ReadMessagesFromUser(userName)).Returns(messagesList);
        _console.Setup(x => x.PrintLn(message));
        
        _twitter.SendCommand(userName);
        
        
        _console.Verify(v => v.PrintLn(message));
    }
}
