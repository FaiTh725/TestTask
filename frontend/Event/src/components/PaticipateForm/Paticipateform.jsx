import { useState } from "react";
import PrimaryButton from "../buttons/PrimaryButton/PrimaryButton";
import ClearInput from "../inputs/ClearInput/ClearInput";
import styles from "./Paticipateform.module.css"
import { useAuth } from "../Auth/AuthContext";
import useLogout from "../../hooks/useLogout";

const PaticipateForm = ({ eventId, handleSendForm, formError}) => {
  const auth = useAuth();
  const logout = useLogout();
  const [ memberForm, setMemberForm] = useState({
    firstName: "",
    secondName: "",
    birthDate: ""
  });

  const handleChangeForm = (e) => {
    const key = e.target.name;
    const newValue = e.target.value;

    setMemberForm(prev => ({
      ...prev,
      [key] : newValue
    }));
  }

  const executeSendForm = () => {
    if(auth.user == null)
    {
      console.log(auth.user);
      console.error("Error ogdfgdfgdfshsdfhdfh");
      return;
    }

    handleSendForm({
      eventId: eventId,
      firstName: memberForm.firstName,
      secondName: memberForm.secondName,
      email: auth.user.email,
      birthDate: memberForm.birthDate
    })
  }
  
  return (
    <div className={styles.PaticipateForm__Main}>
      <h1 className={styles.PaticipateForm__Header}>Input Required Fields</h1>
      <section className={styles.PaticipateForm__InputSection}>
        <p className={styles.PaticipateForm__Field}>First Name</p>
        <ClearInput 
          setValue={handleChangeForm}
          value={memberForm.firstName}
          name="firstName"
          placeHolder="First Name"/>
        <span className={styles.PaticipateForm__FieldError}></span>
      </section>
      <section className={styles.PaticipateForm__InputSection}>
        <p className={styles.PaticipateForm__Field}>Second Name</p>
        <ClearInput
          setValue={handleChangeForm}
          value={memberForm.secondName}
          name="secondName"
          placeHolder="Second Name"/>
        <span className={styles.PaticipateForm__FieldError}></span>
      </section>
      <section className={styles.PaticipateForm__InputSection}>
        <p className={styles.PaticipateForm__Field}>Birth Date</p>
        <ClearInput
          setValue={handleChangeForm}
          value={memberForm.birthDate}
          name="birthDate"
          placeHolder="Birth Date"/>
        <span className={styles.PaticipateForm__FieldError}></span>
      </section>
      <div className={styles.PaticipateForm__Action}>
        <PrimaryButton 
          text={"Continue"} 
          action={() => executeSendForm()}/>
      </div>
    </div>
  )
}

export default PaticipateForm;