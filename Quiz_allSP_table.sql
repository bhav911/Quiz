CREATE PROC GetOptionID  
 @questionID int,  
 @optionOffset tinyint,  
 @optionID int OUTPUT  
AS  
BEGIN  
 select @optionID = optionID  
 from Questions as q  
 inner join Options as o  
 on q.questionID = o.questionID  
 where q.questionID = @questionID  
 order by optionID  
 OFFSET (@optionOffset-1) rows  
 FETCH FIRST 1 ROWS ONLY;  
END



CREATE PROC SubmitUserAnswer  
 @userID int,  
 @quizID int,  
 @questionID int,  
 @optionOffset tinyint  
AS  
BEGIN  
 DECLARE @GetOptionID as int;   
 exec dbo.GetOptionID @questionID, @optionOffset , @optionID = @GetOptionID OUTPUT;   
 Insert into UserAnswers Values ( @userID, @quizID, @questionID, @GetOptionID, GETDATE())  
END



CREATE PROC GetUserCorrectAnswer  
 @quizID int,  
 @userID int  
AS  
BEGIN  
 select SUM(IIF(o.optionID = u.selectedOptionID, 1, 0)) as status  
 from options as o  
 inner join questions as q  
 on o.questionID = q.questionID  
 inner join UserAnswers as u  
 on u.questionID = q.questionID  
 where u.userID = @userID AND o.isCorrect = 1 AND q.quizID = @quizID  
END



CREATE PROC GenerateUserScore  
 @quizID int,  
 @userID int,  
 @userScore int OUTPUT  
AS  
BEGIN  
 select @userScore = SUM(IIF(o.optionID = u.selectedOptionID, 1, 0)) * 5  
 from options as o  
 inner join questions as q  
 on o.questionID = q.questionID  
 inner join UserAnswers as u  
 on u.questionID = q.questionID  
 where u.userID = @userID AND o.isCorrect = 1 AND q.quizID = @quizID  
END



CREATE PROC InsertUserScore    
 @userID int,    
 @quizID int    
AS    
BEGIN    
 Declare @score as int;    
 EXEC dbo.GenerateUserScore  @quizID, @userID, @userScore = @score OUTPUT    
 Insert into Results Values ( @userID, @quizID, @score, GETDATE())    
 select score    
 from Results    
 where quizID = @quizID AND userID = @userID    
END



CREATE PROC InsertUserScore    
 @userID int,    
 @quizID int    
AS    
BEGIN    
 Declare @score as int;    
 EXEC dbo.GenerateUserScore  @quizID, @userID, @userScore = @score OUTPUT    
 Insert into Results Values ( @userID, @quizID, @score, GETDATE())    
 select score    
 from Results    
 where quizID = @quizID AND userID = @userID    
END





CREATE TABLE [dbo].[Admins](
	[adminID] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](30) NULL,
	[email] [varchar](50) NULL,
	[password] [varchar](25) NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
 CONSTRAINT [pk_admins] PRIMARY KEY CLUSTERED 
(
	[adminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]






CREATE TABLE [dbo].[Options](
	[optionID] [int] IDENTITY(1,1) NOT NULL,
	[questionID] [int] NULL,
	[optionText] [text] NULL,
	[isCorrect] [bit] NULL,
	[createdAt] [datetime] NULL,
	[updatedAT] [datetime] NULL,
 CONSTRAINT [pk_options] PRIMARY KEY CLUSTERED 
(
	[optionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Options]  WITH CHECK ADD  CONSTRAINT [fk_questionID_options] FOREIGN KEY([questionID])
REFERENCES [dbo].[Questions] ([questionID])
GO

ALTER TABLE [dbo].[Options] CHECK CONSTRAINT [fk_questionID_options]
GO









CREATE TABLE [dbo].[Questions](
	[questionID] [int] IDENTITY(1,1) NOT NULL,
	[quizID] [int] NULL,
	[questionText] [text] NULL,
	[createdAt] [datetime] NULL,
	[updatedAT] [datetime] NULL,
 CONSTRAINT [pk_questions] PRIMARY KEY CLUSTERED 
(
	[questionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [fk_quizID_questions] FOREIGN KEY([quizID])
REFERENCES [dbo].[Quizzes] ([quizID])
GO

ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [fk_quizID_questions]
GO







CREATE TABLE [dbo].[Quizzes](
	[quizID] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](100) NULL,
	[description] [text] NULL,
	[createdBy] [int] NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
 CONSTRAINT [pk_quiz] PRIMARY KEY CLUSTERED 
(
	[quizID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Quizzes]  WITH CHECK ADD  CONSTRAINT [fk_createdBy_quiz] FOREIGN KEY([createdBy])
REFERENCES [dbo].[Admins] ([adminID])
GO

ALTER TABLE [dbo].[Quizzes] CHECK CONSTRAINT [fk_createdBy_quiz]
GO






CREATE TABLE [dbo].[Results](
	[resultID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[quizID] [int] NULL,
	[score] [int] NULL,
	[createdAt] [datetime] NULL,
 CONSTRAINT [pk_Results] PRIMARY KEY CLUSTERED 
(
	[resultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Results]  WITH CHECK ADD  CONSTRAINT [fk_quizID_Results] FOREIGN KEY([quizID])
REFERENCES [dbo].[Quizzes] ([quizID])
GO

ALTER TABLE [dbo].[Results] CHECK CONSTRAINT [fk_quizID_Results]
GO

ALTER TABLE [dbo].[Results]  WITH CHECK ADD  CONSTRAINT [fk_userID_Results] FOREIGN KEY([userID])
REFERENCES [dbo].[Users] ([userID])
GO

ALTER TABLE [dbo].[Results] CHECK CONSTRAINT [fk_userID_Results]
GO







CREATE TABLE [dbo].[UserAnswers](
	[answerID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[quizID] [int] NULL,
	[questionID] [int] NULL,
	[selectedOptionID] [int] NULL,
	[createdAt] [datetime] NULL,
 CONSTRAINT [pk_UserAnswers] PRIMARY KEY CLUSTERED 
(
	[answerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserAnswers]  WITH CHECK ADD  CONSTRAINT [fk_questionID_UserAnswers] FOREIGN KEY([questionID])
REFERENCES [dbo].[Questions] ([questionID])
GO

ALTER TABLE [dbo].[UserAnswers] CHECK CONSTRAINT [fk_questionID_UserAnswers]
GO

ALTER TABLE [dbo].[UserAnswers]  WITH CHECK ADD  CONSTRAINT [fk_quizID_UserAnswers] FOREIGN KEY([quizID])
REFERENCES [dbo].[Quizzes] ([quizID])
GO

ALTER TABLE [dbo].[UserAnswers] CHECK CONSTRAINT [fk_quizID_UserAnswers]
GO

ALTER TABLE [dbo].[UserAnswers]  WITH CHECK ADD  CONSTRAINT [fk_selectedOptionID_UserAnswers] FOREIGN KEY([selectedOptionID])
REFERENCES [dbo].[Options] ([optionID])
GO

ALTER TABLE [dbo].[UserAnswers] CHECK CONSTRAINT [fk_selectedOptionID_UserAnswers]
GO

ALTER TABLE [dbo].[UserAnswers]  WITH CHECK ADD  CONSTRAINT [fk_userID_UserAnswers] FOREIGN KEY([userID])
REFERENCES [dbo].[Users] ([userID])
GO

ALTER TABLE [dbo].[UserAnswers] CHECK CONSTRAINT [fk_userID_UserAnswers]
GO






CREATE TABLE [dbo].[UserProfile](
	[profileID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[profileContent] [text] NULL,
	[aadharCard] [text] NULL,
	[Marksheet12] [text] NULL,
	[Marksheet10] [text] NULL,
 CONSTRAINT [pk_userProfile] PRIMARY KEY CLUSTERED 
(
	[profileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [fk_userID_userProfie] FOREIGN KEY([userID])
REFERENCES [dbo].[Users] ([userID])
GO

ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [fk_userID_userProfie]
GO









CREATE TABLE [dbo].[Users](
	[userID] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](30) NULL,
	[email] [varchar](50) NULL,
	[password] [varchar](25) NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
 CONSTRAINT [pk_users] PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

