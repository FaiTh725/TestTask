import PrimaryButton from "../../components/buttons/PrimaryButton/PrimaryButton";
import ClearInput from "../../components/inputs/ClearInput/ClearInput";
import CenterScreen from "../../components/layots/CenterScreen/CenterScreen";
import EmptyLink from "../../components/links/EmptyLink/EmptyLink";
import styles from "./Register.module.css";
import logo from "../../assets/Logo.png";
import { useEffect, useState } from "react";
import axios from "axios";
import { useAuth } from "../../components/Auth/AuthContext";
import { useNavigate } from "react-router-dom";

const Register = () => {
  const auth = useAuth();
  const navigate = useNavigate();
  const [credentials, setCredentials] = useState({
    email: "",
    password: "",
    repeatPassword: ""
  });

  const [error, setError] = useState("");

  const handleChangeCredentials = (e) => {
    const key = e.target.name;
    const newValue = e.target.value;

    setCredentials(prev => ({
      ...prev,
      [key]: newValue
    }));
  }

  const handleRegister = async () => {
    let isValidForm = true;

    if(!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.exec(credentials.email))
    {
      setError(prev => prev+"\nInvalid Email");
      isValidForm = false;
    }

    if(!/^(?=.*[A-Za-z])(?=.*\d).+$/.exec(credentials.password))
    {
      setError(prev => prev+"\nPassword should has 1 letter and 1 num");
      isValidForm = false;
    }

    if(credentials.password != credentials.repeatPassword)
    {
      setError(prev => prev+"\nPassword and repeat password are difference");
      isValidForm = false;
    }

    if(!isValidForm)
    {
      return;
    }
    
    try
    {
      var response = await axios.post("https://localhost:7068/api/Authentication/Register", {
        email: credentials.email,
        password: credentials.password
      },
      {
        headers:{
          "Content-Type": "application/json"
        },
        withCredentials: true,
      });

      if(response.data.statusCode === 200)
        {
          localStorage.setItem("user", JSON.stringify(response.data.data));
          auth.login(response.data.data);
          navigate("/");
        }
        else
        {
          setError(response.data.description);
        }
    }
    catch
    {
      console.log("Error send form");
    }
  }

  useEffect(() => {
    if(error != "")
    {
      setError("");
    }
  }, [credentials]);

  return (
    <div className={styles.Register__Content}>

      <CenterScreen>
        <div className={styles.Register__Main}>
          <div className={styles.Register__Logo}>
            <EmptyLink link={"/"}>
              <img src={logo} alt="logo" width={50} height={50}/>
            </EmptyLink>
          </div>
          <h1 className={styles.Register__TopText}>
            Sign in
          </h1>
          <h3 className={styles.Register__SubText}>
            Enter your email and password to Sign in
          </h3>
          <div className={styles.Register__Inputs}>
            <ClearInput 
              setValue={(e) => handleChangeCredentials(e)}
              value={credentials.email} 
              name="email"
              placeHolder="Email"/>
            <ClearInput 
              setValue={(e) => handleChangeCredentials(e)}
              value={credentials.password} 
              name="password"
              placeHolder="Password"
              typeInput="password"/>
            <ClearInput 
              setValue={handleChangeCredentials}
              value={credentials.repeatPassword} 
              name="repeatPassword"
              placeHolder="Repeat Password"
              typeInput="password"/>
          </div>
          <div className={styles.Register__Error}>
            <p>{error}</p>
          </div>
          <PrimaryButton text={"Continue"} action={handleRegister}/>
          <div className={styles.Register__Navigation}>
            <EmptyLink 
              link={"/account/login"}>
              <p>Log in if dont have an account</p>
            </EmptyLink>
          </div>
        </div>
      </CenterScreen>
    </div>
  )
}

export default Register;