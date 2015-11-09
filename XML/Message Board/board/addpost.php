<?php 
	session_start();
	if($_SESSION["state"]){
?>
<html>
<head>
<title>Login Page</title>
<style>
	body{
		width: 700px;
		margin: auto;
	}
</style>
</head>
<body bgcolor="#E6E6FA">
	<?php 
	//Database Connection and Registration
			$dbname = dirname($_SERVER["SCRIPT_FILENAME"]) . "/mydb.sqlite";
			$dbh = new PDO("sqlite:$dbname"); 
	?>
	<?php 
		if($_SERVER['REQUEST_METHOD'] == 'POST'){
			$details = $_POST["details"];
			$id = $_POST["hiddenid"];
			$dbh->beginTransaction();
			if(empty($id)){
				$query = 'insert into posts values("'.uniqid().'","'.$_SESSION["state"].'"," ",datetime(\'now\'),"'.$details.'")';
			}else{
				$query = 'insert into posts values("'.uniqid().'","'.$_SESSION["state"].'","'.$id.'",datetime(\'now\'),"'.$details.'")';
			}
			$dbh->exec($query)
				or die(print_r($dbh->errorInfo(), true));
			$dbh->commit();
			header('Location: board.php');
		}
	?>
	<div id="form-container" style="margin-top: 100px;">
		<a href="board.php" >...Go Back</a><br/>
		<label>Add a post, as a new post or as a reply.</label>
	<form action="addpost.php" method="POST">
		<?php if(isset($_GET["id"])){?>
			<input type="hidden" value="<?php echo $_GET["id"]; ?>" name="hiddenid"/>
		<?php } ?>
		<textarea type="text" rows="10" cols="90" name="details"></textarea><br/>
		<input type="submit" name="submit" value="Post"/>
	</form>
	</div>
</body>
</html>
<?php 
	}else{
		header('Location: login.php');
	}
?>