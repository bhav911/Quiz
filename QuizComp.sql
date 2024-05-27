select * from UserAnswers
select * from options
select * from results
select * from Users
select * from Quizzes
select * from questions

delete from UserAnswers
delete from Results


--CREATE PROC SubmitAnswer
--	@userID

Alter PROC GetOptionID
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

ALter PROC SubmitUserAnswer
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


alter PROC InsertUserScore
	@userID int,
	@quizID int
AS
BEGIN
	Declare @score as int;
	EXEC dbo.GenerateUserScore @userID, @quizID, @userScore = @score OUTPUT
	Insert into Results Values ( @userID, @quizID, @score, GETDATE())
	select score
	from Results
	where quizID = @quizID AND userID = @userID
END

delete from results



exec dbo.InsertUserScore @userID =1, @quizID=1



delete from results


select COUNT(selectedOptionID)
from UserAnswers as u
inner join Questions as que
on u.questionID = que.questionID
inner join Quizzes as q
on que.quizID = q.quizID
where u.userID = 1 AND selectedOptionID IN (select optionID
											from Options as o
											inner join Questions as q
											on o.questionID = q.questionID
											where q.quizID = 1 AND o.isCorrect = 1)

Declare @count as int;

ALTER PROC GenerateUserScore
	@quizID int,
	@userID int,
	@userScore int OUTPUT
AS
BEGIN
	select o.optionID, u.selectedOptionID 
	from options as o
	inner join questions as q
	on o.questionID = q.questionID
	inner join UserAnswers as u
	on u.questionID = q.questionID
	where u.userID = 1 AND o.isCorrect = 1 AND q.quizID = 1
END






