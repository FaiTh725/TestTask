import PrimaryButton from "../../components/buttons/PrimaryButton/PrimaryButton";
import ClearInput from "../../components/inputs/ClearInput/ClearInput";
import CenterScreen from "../../components/layots/CenterScreen/CenterScreen";
import EmptyLink from "../../components/links/EmptyLink/EmptyLink";
import styles from "./Register.module.css";

const Register = () => {
  return (
    <CenterScreen>
      <div className={styles.Register__Main}>
        <h1 className={styles.Register__TopText}>
          Sign in
        </h1>
        <h3 className={styles.Register__SubText}>
          Enter your email and password to Sign in
        </h3>
        <div className={styles.Register__Inputs}>
          <ClearInput placeHolder="Email"/>
          <ClearInput placeHolder="Password"/>
        </div>
        <div className={styles.Register__Error}>
        
        </div>
        <PrimaryButton text={"Continue"}/>
        <div className={styles.Register__Navigation}>
          <EmptyLink 
            link={"/account/login"} 
            text={"Log in if dont have an account"}/>
        </div>
      </div>
    </CenterScreen>
  )
}

export default Register;