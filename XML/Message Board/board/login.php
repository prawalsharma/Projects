<?php 
	session_start();
?>
<html>
<head>
<title>Login Page</title>
<style>
	body{
		width: 700px;
		margin: auto;
	}
	#register-form{
		margin-top: 100px;
	}
	#login-form{
		margin-top: 100px;
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
		if(isset($_POST["login"])){
			$uname = $_POST["uname"];
			$pass = $_POST["pass"];
			$statement = 'SELECT * FROM users WHERE username=:username';
			$stmnt = $dbh->prepare($statement);
			$stmnt->execute(array(':username'=>$uname));
			$entry = $stmnt->fetch();
			if($entry['username'] == $uname && $entry['password'] == md5($pass)){
				$_SESSION["state"] = $uname;
				header('Location: board.php');
			}else{
				echo "Username and Password does not match";
			}
		}
	?>
	<div id="login-form">
		<form method="POST" action="login.php">
		<fieldset style="background-color: white;">
			<legend>Login:</legend>
			<table align="center">
				<tr>
					<td>Username: </td>
					<td><input type="text" name="uname"/></td>
				</tr>
				<tr>
					<td>Password:</td>
					<td><input type="password" name="pass"/></td> 
				</tr>
				<tr>
					<td></td>
					<td><input type="Submit" name="login" value="Login"/></td>
				</tr>
			</table>
		</fieldset>
		</form>
		<form method="GET" action="register.php">
			<input type="submit" value="New users must register here"/>
		</form>
	</div>
	<?php 
		if(isset($_GET["message"])){
			echo $_GET["message"];
		}
	?>
	
</body>
</html>