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
</style>
</head>
<body bgcolor="#E6E6FA">
	<?php 
	//Database Connection and Registration
			$dbname = dirname($_SERVER["SCRIPT_FILENAME"]) . "/mydb.sqlite";
			$dbh = new PDO("sqlite:$dbname"); 
	?>
	<a href="login.php">..Back</a>
	<div id="register-form">
	<form method="POST" action="register.php" >
	<fieldset>
		<legend>New Users must Register here:</legend>
		<table align="center">
			<tr>
				<td>Username:</td>
				<td><input type="text" name="username" /></td>
			</tr>
			<tr>
				<td>Password:</td>
				<td><input type="password" name="password" /></td>
			</tr>
			<tr>
				<td>Full Name:</td>
				<td><input type="text" name="fullname" /></td>
			</tr>
			<tr>
				<td>Email:</td>
				<td><input type="text" name="email" /></td>
			</tr>
			<tr>
				<td></td>
				<td><input type="submit" name="register" value="Register"/></td> 
			</tr>
		</table>
	</form>
	</div>
	<?php 
		if(isset($_POST["register"])){
			$username = $_POST["username"];
			$password = $_POST["password"];
			$full_name = $_POST["fullname"];
			$email = $_POST["email"];
			//Retrieving previous user and Registration
			$statement = 'SELECT * FROM users WHERE username=:username';
			$stmt = $dbh->prepare($statement);
			$stmt->execute(array(':username'=> $username));
			$entry = $stmt->fetch();
			if($entry['username'] == $username){
				echo "Username already exists";
			}else{
				$dbh->exec('insert into users values("' .$username. '","' . md5($password) . '","'.$full_name.'","'.$email.'")')
					or die(print_r($dbh->errorInfo(), true));
				header('Location: login.php?message=Successfully Registered. Login to continue');
			}
		}
	?>
</body>
</html>