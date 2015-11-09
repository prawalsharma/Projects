<!-- Name: Prawal Sharma
	Link: omega.uta.edu/~pss4720/project4/login.php
	UTA ID: 1001104720 -->
<?php 
	session_start();
	if($_SESSION["state"]){
?><html>
<head>
<title>Message Board</title>
<style>
	body{
		width: 1000px;
		margin: auto;
	}
	#messages{
		border: 1px solid;
	}
	#message-header{
		width: 100%;
		height: 35px;
		background-color: BurlyWood;
	}
	#message-content{
		width: 100%;
		height: auto;
		min-height: 50px;
		background-color: white;
	}
	h4,h5{
		margin: 0px;
		padding: 0px;
	}
</style>
</head>
<body bgcolor="#E6E6FA">
	<?php 
		if(isset($_GET["logout"])){
			unset($_SESSION["state"]);
			header('Location: login.php');
		}
	?>
	<?php 
		try{
		$dbname = dirname($_SERVER["SCRIPT_FILENAME"]) . "/mydb.sqlite";
		$dbh = new PDO("sqlite:$dbname");
		$stmt = $dbh->prepare('select * from posts p,users u where p.postedby = u.username ORDER BY p.datetime DESC');
		$stmt->execute();
		}catch(PDOException $e){
			print "Error!: " . $e->getMessage() . "<br/>";
			die();
		}
	?>
	<div id="menu" style="margin-top: 10px;">
		<form action="board.php" method="GET">
			<input type="submit" name="logout" value="Logout" style="float: right;"/>
		</form>
		<a href="addpost.php">New Message</a>
	</div><br/>
	<?php 
		while ($row = $stmt->fetch()) { ?>
			<div id="messages">
				<div id="message-header">
					<div id="date" style="float: right; padding-right: 15px; padding-top: 10px;"><?php echo $row["datetime"]; ?></div>
					<div id="name" style="padding-left: 10px;"><h4><?php echo $row["fullname"]; ?></h4></div>
					<div id="username" style="padding-left: 10px;"><h5><?php echo $row["postedby"]; ?></h5></div>
				</div>
				<div id="message-content">
					<span style="font-size: 14px; padding-left: 5px;">Message ID: <?php echo $row["id"];?>
						<?php if($row["follows"] != " "){ 
							echo " , Reply to: ".$row["follows"];
						}?>
					</span>
					<a href="addpost.php?id=<?php echo $row["id"]; ?>" style="float: right;">Reply</a>
					<p style="padding-left: 10px;"><?php echo $row["message"]; ?></p>
				</div>
			</div>
			<?php } ?>
	<?php
	error_reporting(E_ALL);
	ini_set('display_errors','On');
	?>
</body>
</html>
<?php 
	}else{
		header('Location: login.php');
	}
?>
